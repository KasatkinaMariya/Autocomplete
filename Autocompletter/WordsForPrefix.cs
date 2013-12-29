using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autocompletter
{
    public class WordsForPrefix
    {
        public string Prefix { get; set; }
        public IEnumerable<string> Words { get; set; }

        public WordsForPrefix(string prefix, IEnumerable<string> words)
        {
            Prefix = prefix;
            Words = words;
        }

        public string FormDefinitionString()
        {
            var definitionBuilder = new StringBuilder();

            definitionBuilder.AppendFormat("prefix='{0}' ", Prefix)
                             .AppendFormat("count='{0}' ", Words.Count());

            definitionBuilder.Append("words=(");
            foreach (var word in Words)
                definitionBuilder.AppendFormat("'{0}' ", word);
            definitionBuilder.Append(")");

            return definitionBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var another = obj as WordsForPrefix;
            if (another == null)
                return false;

            return this.Prefix == another.Prefix
                   && this.Words.SequenceEqual(another.Words);
        }

        public override int GetHashCode()
        {
            return Prefix.GetHashCode();
        }
    }
}