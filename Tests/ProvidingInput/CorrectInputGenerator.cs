using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autocompletter;

namespace Tests.ProvidingInput
{
    static class CorrectInputGenerator
    {
        private static readonly Random _st_random = new Random();

        private const int _c_minSymbolCode = 'a';
        private const int _c_maxSymbolCode = 'z';

        public static IEnumerable<VocabularyItem> GenerateVocabulary(InputSettings settings)
        {
            var words = new HashSet<string>();
            while (words.Count < settings.VocabularySize)
                words.Add(GenerateOneWord(settings.WordMaxLength));

            Func<int> genOccurrence = () => _st_random.Next(settings.WordMaxOccurrences + 1);
            return words
                   .Select(word => new VocabularyItem(word, genOccurrence()))
                   .ToArray();
        }

        public static IList<string> GeneratePrefixes(InputSettings settings)
        {
            var prefixes = new List<string>(settings.PrefixesNumber);

            for (int i=0; i < settings.PrefixesNumber; i++)
                prefixes.Add(GenerateOneWord(settings.PrefixMaxLength));

            return prefixes;
        }

        private static string GenerateOneWord(int maxLength)
        {
            int desiredLength = _st_random.Next(1, maxLength + 1);
            var resultWord = new StringBuilder(desiredLength);

            while (resultWord.Length < desiredLength)
            {
                int curSymbolCode = _st_random.Next(_c_minSymbolCode, _c_maxSymbolCode + 1);
                resultWord.Append((char)curSymbolCode);
            }

            return resultWord.ToString();
        }
    }
}
