using System;
using System.Collections.Generic;
using System.Linq;
using Autocompletter;
using AutocompletterSettings = Autocompletter.Properties.Settings;

namespace Tests.Validation
{
    class CorrectAutocompletter : IAutocompletter
    {
        private readonly SortedDictionary<string, int> _wordToOccurence;
        private readonly int _maxWordsNumberToReturn;

        public void FillVocabulary(IEnumerable<VocabularyItem> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                _wordToOccurence.Add(item.Word, item.WordOccurrence);
        }

        public IEnumerable<WordsForPrefix> GetWordsForPrefixes(IEnumerable<string> prefixes)
        {
            Func<string, IEnumerable<string>> collectWordsForOnePrefix =
                (prefix) => (from item in _wordToOccurence
                             where item.Key.StartsWith(prefix)
                             orderby item.Value descending, item.Key
                             select item.Key)
                            .Take(_maxWordsNumberToReturn);

            return from prefix in prefixes
                   select new WordsForPrefix(prefix, collectWordsForOnePrefix(prefix));
        }

        public CorrectAutocompletter()
        {
            _wordToOccurence = new SortedDictionary<string, int>();
            _maxWordsNumberToReturn = AutocompletterSettings.Default
                                      .MaxWordsNumberToReturn;
        }
    }
}
