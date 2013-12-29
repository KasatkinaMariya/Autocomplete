using System.Collections.Generic;
using System.Linq;
using AutocompletterSettings = Autocompletter.Properties.Settings;

namespace Autocompletter
{
    public abstract class AutocompletterBase : IAutocompletter
    {
        protected abstract void AddItemToVocabulary(VocabularyItem item);
        protected abstract IEnumerable<string> GetMostPopularWords(string prefix);

        public void FillVocabulary(IEnumerable<VocabularyItem> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                AddItemToVocabulary(item);
        }


        public IEnumerable<WordsForPrefix> GetWordsForPrefixes(IEnumerable<string> prefixes)
        {
            bool useParallelization = prefixes.Count() >= Configuration.Default.MinPrefixesNumberForConcurrentProcessing;

            var prefixToMostPopularWords = useParallelization
                                           ? prefixes
                                             .AsParallel().AsOrdered()
                                             .Select(prefix => new WordsForPrefix(prefix, GetMostPopularWords(prefix)))
                                           : prefixes
                                             .Select(prefix => new WordsForPrefix(prefix, GetMostPopularWords(prefix)));
            return prefixToMostPopularWords.ToArray();
        }
    }
}
