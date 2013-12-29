using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autocompletter;

namespace Tests
{
    static class Utils
    {
        public static void SaveInputToFile(string pathToFile,
                                           IEnumerable<VocabularyItem> vocabulary,
                                           IEnumerable<string> prefixes)
        {
            var inputAccumulator = new StringBuilder();

            inputAccumulator.AppendLine(vocabulary.Count().ToString());
            foreach (VocabularyItem item in vocabulary)
                inputAccumulator.AppendFormat("{0} {1}", item.Word, item.WordOccurrence)
                                .AppendLine();

            inputAccumulator.AppendLine(prefixes.Count().ToString());
            foreach (string prefix in prefixes)
                inputAccumulator.AppendLine(prefix);

            File.WriteAllText(pathToFile,inputAccumulator.ToString());
        }
    }
}
