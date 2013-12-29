using TestSettings = Tests.Properties.Settings;

namespace Tests.ProvidingInput
{
    class InputSettings
    {
        public int VocabularySize { get; set; }
        public int WordMaxLength { get; set; }
        public int WordMaxOccurrences { get; set; }

        public int PrefixesNumber { get; set; }
        public int PrefixMaxLength { get; set; }

        public string FormDescriptionString()
        {
            return string.Format("VocabularySize={0} WordLength={1} PrefixesNumber={2} PrefixLength={3}",
                                  VocabularySize, WordMaxLength, PrefixesNumber, PrefixMaxLength);
        }

        public static InputSettings GetFromAppSettings()
        {
            return new InputSettings()
                {
                    VocabularySize = TestSettings.Default.VocabularySize,
                    WordMaxLength = TestSettings.Default.WordMaxLength,
                    WordMaxOccurrences = TestSettings.Default.WordMaxOccurrences,
                    PrefixesNumber = TestSettings.Default.PrefixesNumber,
                    PrefixMaxLength = TestSettings.Default.PrefixMaxLength
                };
        }
    }
}