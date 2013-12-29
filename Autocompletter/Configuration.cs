using Autocompletter.Properties;

namespace Autocompletter
{
    public class Configuration
    {
        public static Configuration Default;

        public int MinPrefixesNumberForConcurrentProcessing { get; private set; }
        public int MaxWordsNumberToReturn { get; private set; }

        public int MinSymbolCode { get; private set; }
        public int VariousSymbolsCount { get; private set; }
        public int MaxWordLength { get; private set; }

        static Configuration()
        {
            Default = new Configuration()
                {
                    MinPrefixesNumberForConcurrentProcessing = Settings.Default.MinPrefixesNumberForConcurrentProcessing,
                    MaxWordsNumberToReturn = Settings.Default.MaxWordsNumberToReturn,

                    MinSymbolCode = Settings.Default.MinSymbolCode,
                    VariousSymbolsCount = Settings.Default.VariousSymbolsNumber,
                    MaxWordLength = Settings.Default.MaxWordLength
                };
        }
    }
}
