using System.Collections.Generic;
using Autocompletter;

namespace AutocompleteUser.SavingAnswer
{
    public interface IAnswerSaver
    {
        void SaveAnswer(IEnumerable<WordsForPrefix> wordsForPrefixes);
    }
}
