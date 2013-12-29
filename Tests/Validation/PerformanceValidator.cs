using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AutocompleteUser.ReadingInput;
using AutocompleteUser.SavingAnswer;
using Autocompletter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.ProvidingInput;
using TestSettings = Tests.Properties.Settings;

namespace Tests.Validation
{
    class PerformanceValidator : ValidatorBase
    {
        private long _fileReadingTimeInMills;
        private long _vocabularyParsingTimeInMills;
        private long _prefixesParsingTimeInMills;
        private long _fillingTimeInMills;
        private long _answeringTimeInMills;
        private long _fileWritingTimeInMills;

        private long _totalWorkTimeInMills;

        public override void Validate()
        {
            var pathToTempInputFile = Path.GetTempFileName();
            var pathToTempOutputFile = Path.GetTempFileName();
            Utils.SaveInputToFile(pathToTempInputFile,_vocabulary, _prefixes);

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var inputReader = new InputFileReader(pathToTempInputFile);
                _fileReadingTimeInMills = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                var vocabulary = inputReader.ReadVocabulary();
                _vocabularyParsingTimeInMills = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                var prefixes = inputReader.ReadPrefixes();
                _prefixesParsingTimeInMills = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                var autocompletter = new TrieAutocompletter();
                autocompletter.FillVocabulary(vocabulary);
                _fillingTimeInMills = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                var wordsForPrefixes = autocompletter.GetWordsForPrefixes(prefixes);
                _answeringTimeInMills = stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                var answerSaver = new FileAnswerSaver(pathToTempOutputFile);
                answerSaver.SaveAnswer(wordsForPrefixes);
                _fileWritingTimeInMills = stopwatch.ElapsedMilliseconds;

                _totalWorkTimeInMills = _fileReadingTimeInMills + _vocabularyParsingTimeInMills +
                                        _prefixesParsingTimeInMills
                                        + _fillingTimeInMills + _answeringTimeInMills + _fileWritingTimeInMills;
                SaveMetricsInfoToFile();

                Assert.IsTrue(_totalWorkTimeInMills < TestSettings.Default.MaxTotalTimeInMills,
                              string.Format("Total work time for '{0}' is {1} ms." +
                                            " It's greater than max permissible ({2} ms)",
                                            _inputSettings.FormDescriptionString(),
                                            _totalWorkTimeInMills, TestSettings.Default.MaxTotalTimeInMills));
            }
            finally
            {
                File.Delete(pathToTempInputFile);
                File.Delete(pathToTempOutputFile);
            }
        }

        private void SaveMetricsInfoToFile()
        {
            var metricInfoToSave = new List<string>()
                {
                    DateTime.Now.ToString(),
                    _inputSettings.FormDescriptionString(),
                    string.Format("reading file\t\t{0} ms", _fileReadingTimeInMills),
                    string.Format("parsing vocabulary\t{0} ms\t\t{1} i/ms",
                                   _vocabularyParsingTimeInMills, (double)_vocabulary.Count()/_vocabularyParsingTimeInMills),
                    string.Format("parsing prefixes\t{0} ms\t\t{1} p/ms",
                                   _prefixesParsingTimeInMills, (double)_prefixes.Count()/_prefixesParsingTimeInMills),
                    string.Format("filling vocabulary\t{0} ms\t\t{1} i/ms",
                                   _fillingTimeInMills, (double)_vocabulary.Count()/_fillingTimeInMills),
                    string.Format("calculating answer\t{0} ms\t\t{1} p/ms",
                                   _answeringTimeInMills, (double)_prefixes.Count()/_answeringTimeInMills),
                    string.Format("saving answer\t\t{0} ms", _fileWritingTimeInMills),
                    string.Format("total work\t\t\t{0} ms", _totalWorkTimeInMills)
                };

            var metricsAccumulator = new StringBuilder();
            foreach (var infoLine in metricInfoToSave)
                metricsAccumulator.AppendLine(infoLine);
            metricsAccumulator.AppendLine();

            File.AppendAllText(TestSettings.Default.PathToPerformanceMetricsFile,
                               metricsAccumulator.ToString());
        }

        public PerformanceValidator (InputSettings inputSettings,
                                     IEnumerable<VocabularyItem> vocabulary,
                                     IEnumerable<string> prefixes)
            : base (inputSettings, vocabulary, prefixes)
        {
        }
    }
}
