using System;
using System.IO;
using AutocompleteUser.Properties;
using AutocompleteUser.ReadingInput;
using AutocompleteUser.SavingAnswer;
using Autocompletter;

namespace AutocompleteUser
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputReader = new InputFileReader(Settings.Default.PathToInputFile);
            var vocabulary = inputReader.ReadVocabulary();
            var prefixes = inputReader.ReadPrefixes();

            var autocompletter = new TrieAutocompletter();
            autocompletter.FillVocabulary(vocabulary);
            var wordsForPrefixes = autocompletter.GetWordsForPrefixes(prefixes);

            var answerSaver = new FileAnswerSaver(Settings.Default.PathToOutputFile);
            answerSaver.SaveAnswer(wordsForPrefixes);

            foreach (var a in File.ReadAllLines(Settings.Default.PathToOutputFile))
            {
                Console.WriteLine(a);
            }
        }
    }
}