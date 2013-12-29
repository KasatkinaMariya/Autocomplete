using System.Collections.Generic;
using Autocompletter;

namespace AutocompleteUser.ReadingInput
{
    public interface IInputReader
    {
        IEnumerable<VocabularyItem> ReadVocabulary();
        IEnumerable<string> ReadPrefixes();
    }
}
