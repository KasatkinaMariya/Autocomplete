using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autocompletter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.ProvidingInput;

namespace Tests.Validation
{
    class CorrectnessValidator : ValidatorBase
    {
        public override void Validate()
        {
            var myAutocompletter = new TrieAutocompletter();
            myAutocompletter.FillVocabulary(_vocabulary);
            var _actualAnswer = myAutocompletter.GetWordsForPrefixes(_prefixes).ToArray();

            var rightAutocompletter = new CorrectAutocompletter();
            rightAutocompletter.FillVocabulary(_vocabulary);
            var _expectedAnswer = rightAutocompletter.GetWordsForPrefixes(_prefixes).ToArray();

            Assert.AreEqual(_prefixes.Count(), _actualAnswer.Count(),
                            string.Format("Count ofword's sets in reply is {0}," +
                                          " and isn't equal to {1} (number of prefixes)",
                                           _actualAnswer.Count(), _prefixes.Count()));

            Action<int> checkOnePrefix =
                (index) =>
                    {
                        var notEqualMessage =
                            string.Format("Set №{0} in reply. There shouls be {1}, but really there is {2}",
                                           index, _expectedAnswer[index].FormDefinitionString(),
                                           _actualAnswer[index].FormDefinitionString());
                        Assert.AreEqual(_expectedAnswer[index], _actualAnswer[index], notEqualMessage);
                    };
            Parallel.For(0, _prefixes.Count(), checkOnePrefix);
        }

        public CorrectnessValidator (InputSettings inputSettings,
                                     IEnumerable<VocabularyItem> vocabulary,
                                     IEnumerable<string> prefixes)
            : base (inputSettings, vocabulary, prefixes)
        {
        }
    }
}
