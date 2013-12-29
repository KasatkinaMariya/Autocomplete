using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autocompletter;

namespace AutocompleteUser.ReadingInput
{
    public class InputFileReader : IInputReader
    {
        private readonly string _pathToInputFile;
        private readonly string[] _inputLines;

        private readonly int _vocabularySize;
        private readonly int _prefixesNumber;

        private static readonly char[] _st_whitespaceSymbols = { ' ', '\t' };

        private const string _c_vocabularyLinePattern =
            "^\\s*" +           // any whitespaces in the beginning aren't a problem
            "[a-zA-Z]{1,15}" +  // word, upper case may be reduce to lower later
            "\\s+" +            // one space splitting word and occurrence; several whitespaces are legal
            "\\d{1,7}" +        // word's occurrence
            "\\s*$";            // any whitespaces at the end aren't a problem
        private const string _c_prefixLinePattern =
            "^\\s*" +
            "[a-zA-Z]{1,15}" +
            "\\s*$";

        public IEnumerable<VocabularyItem> ReadVocabulary()
        {
            var vocabularyItems = new ConcurrentBag<VocabularyItem>();

            Parallel.For(1, _vocabularySize + 1,
                        (vocabularyLineIndex) =>
                        {
                            var curLine = _inputLines[vocabularyLineIndex];
                            if (!Regex.IsMatch(curLine, _c_vocabularyLinePattern))
                                return;

                            var splitted = curLine.Split(_st_whitespaceSymbols, StringSplitOptions.RemoveEmptyEntries);
                            string word = splitted[0].ToLower();
                            int occurrence = int.Parse(splitted[1]);

                            vocabularyItems.Add(new VocabularyItem(word, occurrence));
                        }
            );

            return vocabularyItems;
        }

        public IEnumerable<string> ReadPrefixes()
        {
            var prefixes = new List<string>(_prefixesNumber);

            var prefixesFirstLineNumber = _vocabularySize + 2;
            for (int i = 0; i < _prefixesNumber; i++)
            {
                var curLine = _inputLines[i + prefixesFirstLineNumber];
                if (!Regex.IsMatch(curLine, _c_prefixLinePattern))
                    continue;

                prefixes.Add(curLine.Trim().ToLower());
            }

            return prefixes;
        }

        public InputFileReader(string pathToInputFile)
        {
            _pathToInputFile = pathToInputFile;
            if (!File.Exists(_pathToInputFile))
            {
                var noFileMessage = string.Format("Path '{0}' doesn't exist. Correct value" +
                                                  " of 'PathToInputFile' in App.config",
                                                  _pathToInputFile);
                throw new InvalidInputFileException(noFileMessage, _pathToInputFile);
            }

            _inputLines = FileStructureController.LoadAndCheck(_pathToInputFile);
            _vocabularySize = int.Parse(_inputLines[0]);
            _prefixesNumber = int.Parse(_inputLines[_vocabularySize + 1]);
        }
    }
}
