using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autocompletter;

namespace AutocompleteUser.SavingAnswer
{
    public class FileAnswerSaver : IAnswerSaver
    {
        private readonly string _pathToOutputFile;

        public void SaveAnswer(IEnumerable<WordsForPrefix> wordsForPrefixes)
        {
            var answerAccumulator = new StringBuilder();

            foreach (var wordsForPrefix in wordsForPrefixes)
            {
                if (wordsForPrefix.Words.Any())
                {
                    foreach (var word in wordsForPrefix.Words)
                        answerAccumulator.AppendLine(word);
                    answerAccumulator.AppendLine();
                }
            }

            var fullAnswerStr = answerAccumulator.ToString().TrimEnd();
            File.WriteAllText(_pathToOutputFile, fullAnswerStr);
        }

        public FileAnswerSaver(string pathToOutputFile)
        {
            _pathToOutputFile = pathToOutputFile;
        }
    }
}