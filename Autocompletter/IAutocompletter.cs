using System.Collections.Generic;

namespace Autocompletter
{
    public interface IAutocompletter
    {
        void FillVocabulary(IEnumerable<VocabularyItem> itemsToAdd);
        IEnumerable<WordsForPrefix> GetWordsForPrefixes(IEnumerable<string> prefixes);
    }
}
