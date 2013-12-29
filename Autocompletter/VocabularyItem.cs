using System;
using System.Diagnostics;
using System.Text;
using AutocompletterSettings = Autocompletter.Properties.Settings;

namespace Autocompletter
{
    [DebuggerTypeProxy(typeof(VocabularyItemDebuggerProxy))]
    public class VocabularyItem : IComparable<VocabularyItem>
    {
        public string Word { get; private set; }
        public int WordOccurrence { get; private set; }

        public double WordHash
        {
            get
            {
                //lock (_o)
                return _wordHash ?? (_wordHash = CalculateHash()).Value;
            }
        }
        private double? _wordHash;

        private static readonly int _st_hashBasis = 1 + Configuration.Default.VariousSymbolsCount;
        private static readonly char _st_aligningSymbol = (char)(-1 + Configuration.Default.MinSymbolCode);


        public VocabularyItem(string word, int occurrence)
        {
            Word = word;
            WordOccurrence = occurrence;
        }

        public int CompareTo(VocabularyItem other)
        {
            var occurrenceDiff = other.WordOccurrence - WordOccurrence;
            if (occurrenceDiff != 0)
                return occurrenceDiff;

            var hashDiff = WordHash - other.WordHash;
            return Math.Sign(hashDiff);
        }

        private double CalculateHash()
        {
            var alignedWordBuilder = new StringBuilder(Word);
            while (alignedWordBuilder.Length < Configuration.Default.MaxWordLength)
                alignedWordBuilder.Append(_st_aligningSymbol);
            var alignedWord = alignedWordBuilder.ToString();

            double hashAccumulator = 0;
            double curPow = 1;
            for (int i = alignedWord.Length - 1; i >= 0; i--)
            {
                hashAccumulator += curPow * (alignedWord[i] - Configuration.Default.MinSymbolCode);
                curPow *= _st_hashBasis;
            }

            return hashAccumulator;
        }

        private class VocabularyItemDebuggerProxy
        {
            private readonly VocabularyItem _item;

            public string Word
            {
                get { return _item.Word; }
            }

            public int WordOccurrence
            {
                get { return _item.WordOccurrence; }
            }

            public double? WordHash
            {
                get { return _item._wordHash; }
            }

            public VocabularyItemDebuggerProxy(VocabularyItem item)
            {
                _item = item;
            }
        }
    }
}