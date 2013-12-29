using System;
using System.IO;
using System.Linq;

namespace AutocompleteUser.ReadingInput
{
    static class FileStructureController
    {
        public static string[] LoadAndCheck(string pathToInputFile)
        {
            var inputLines = File.ReadAllLines(pathToInputFile);

            if (!inputLines.Any())
            {
                var noContentMessage = string.Format("No content was found in '{0}'." +
                                                     " Fill vocabulary and prefixes",
                                                     pathToInputFile);
                throw new InvalidInputFileException(noContentMessage, pathToInputFile);
            }

            int vocabularySize;
            try
            {
                vocabularySize = int.Parse(inputLines[0]);
            }
            catch (FormatException e)
            {
                var message = string.Format("First line is '{0}' and can't be parsed as integer." +
                                            " Put size of vocabulary here",
                                            inputLines[0]);
                throw new InvalidInputFileException(message, pathToInputFile, 1, inputLines[0], e);
            }

            int prefixesNumber;
            try
            {
                prefixesNumber = int.Parse(inputLines[vocabularySize + 1]);
            }
            catch (FormatException e)
            {
                var message = string.Format("N+2 line is '{0}' and can't be parsed as integer." +
                                            " Put number of prefixes here",
                                            inputLines[vocabularySize + 1]);
                throw new InvalidInputFileException(message, pathToInputFile,
                                                    vocabularySize + 2, inputLines[vocabularySize + 1], e);
            }

            int minPossibleLinesCount = vocabularySize + prefixesNumber + 2;
            if (inputLines.Length < minPossibleLinesCount)
            {
                var tooLessLinesCountMessage =
                    string.Format("If vocabulary size is {0} and number of prefixes is {1}," +
                                  " then count of lines should be eq to {2} but there are only {3} lines",
                                  vocabularySize, prefixesNumber, minPossibleLinesCount, inputLines.Length);
                throw new InvalidInputFileException(tooLessLinesCountMessage, pathToInputFile);
            }

            return inputLines;
        }
    }
}
