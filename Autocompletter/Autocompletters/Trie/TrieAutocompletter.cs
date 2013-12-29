using System;
using System.Collections.Generic;
using System.Linq;
using AppSettings = Autocompletter.Properties.Settings;

namespace Autocompletter
{
    public class TrieAutocompletter : AutocompletterBase
    {
        private readonly TrieNode _root;
        
        public TrieAutocompletter()
        {
            _root = new TrieNode();
        }

        protected override void AddItemToVocabulary(VocabularyItem toAdd)
        {
            TrieNode curTrieNode = _root;
            
            foreach (char wordSymbol in toAdd.Word)
            {
                if (curTrieNode[wordSymbol] == null)
                {
                    curTrieNode[wordSymbol] = new TrieNode();
                }
                curTrieNode = curTrieNode[wordSymbol];
            }

            curTrieNode.VocabularyItem = new VocabularyItem(toAdd.Word, toAdd.WordOccurrence);
        }

        protected override IEnumerable<string> GetMostPopularWords(string prefix)
        {
            TrieNode prefixNode;
            
            if (!_root.TryGetValue(prefix, out prefixNode))
                return Enumerable.Empty<string>();

            return RelevantNodesCollector.Collect(prefixNode)
                   .Select(item => item.Word);

        }
    }

    public static class RelevantNodesCollector
    {
        [ThreadStatic]
        private static SortedLinkedList<VocabularyItem> _itemsCollectedInCurThread;

        public static SortedLinkedList<VocabularyItem> Collect(TrieNode root)
        {
            _itemsCollectedInCurThread = new SortedLinkedList<VocabularyItem>
                                                (Configuration.Default.MaxWordsNumberToReturn);
            CollectFromNode(root);
            return _itemsCollectedInCurThread;
        }

        private static void CollectFromNode(TrieNode trieNode)
        {
            if (trieNode.VocabularyItem != null)
                _itemsCollectedInCurThread.Add(trieNode.VocabularyItem);

            foreach (TrieNode node in trieNode.NextSymbols)
            {
                if (node != null)
                    CollectFromNode(node);
            }
        }
    }
}
