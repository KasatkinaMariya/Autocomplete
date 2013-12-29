using System.Collections.Generic;
using Autocompletter;
using Tests.ProvidingInput;
using TestSettings = Tests.Properties.Settings;

namespace Tests.Validation
{
    abstract class ValidatorBase
    {
        protected readonly InputSettings _inputSettings;
        protected readonly IEnumerable<VocabularyItem> _vocabulary;
        protected readonly IEnumerable<string> _prefixes;

        public abstract void Validate();

        protected ValidatorBase(InputSettings inputSettings,
                                IEnumerable<VocabularyItem> vocabulary,
                                IEnumerable<string> prefixes)
        {
            _inputSettings = inputSettings;
            _vocabulary = vocabulary;
            _prefixes = prefixes;
        }
    }
}