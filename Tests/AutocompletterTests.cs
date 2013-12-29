using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.ProvidingInput;
using Tests.Validation;
using TestSettings = Tests.Properties.Settings;

namespace Tests
{
    [TestClass]
    public class AutocompletterTests
    {
        [TestMethod]
        public void MaxLoadShortPrefixesTest()
        {
            var settings = new InputSettings()
                {
                    VocabularySize = 100000,
                    WordMaxLength = 15,
                    WordMaxOccurrences = 1000000,
                    PrefixesNumber = 15000,
                    PrefixMaxLength = 2
                };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void MaxLoadAveragePrefixesTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 100000,
                WordMaxLength = 15,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 15000,
                PrefixMaxLength = 6
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void MaxLoadLongPrefixesTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 100000,
                WordMaxLength = 15,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 15000,
                PrefixMaxLength = 15
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void MaxLoadSeveralRelevantWordsTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 100000,
                WordMaxLength = 4,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 15000,
                PrefixMaxLength = 15
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void LowLoadShortPrefixesTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 10000,
                WordMaxLength = 15,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 1500,
                PrefixMaxLength = 2
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void LowLoadAveragePrefixesTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 10000,
                WordMaxLength = 15,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 1500,
                PrefixMaxLength = 6
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void LowLoadLongPrefixesTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 10000,
                WordMaxLength = 15,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 1500,
                PrefixMaxLength = 15
            };

            TestOneSettings(settings);
        }

        [TestMethod]
        public void LowLoadSeveralRelevantWordsTest()
        {
            var settings = new InputSettings()
            {
                VocabularySize = 10000,
                WordMaxLength = 4,
                WordMaxOccurrences = 1000000,
                PrefixesNumber = 1500,
                PrefixMaxLength = 15
            };

            TestOneSettings(settings);
        }

        private void TestOneSettings(InputSettings settings = null)
        {
            if (settings == null)
                settings = InputSettings.GetFromAppSettings();

            var vocabulary = CorrectInputGenerator.GenerateVocabulary(settings);
            var prefixes = CorrectInputGenerator.GeneratePrefixes(settings);

            if (TestSettings.Default.ValidateCorrectness)
                (new CorrectnessValidator(settings, vocabulary, prefixes)).Validate();

            if (TestSettings.Default.ValidatePerformance)
                (new PerformanceValidator(settings, vocabulary, prefixes)).Validate();

        }
    }
}