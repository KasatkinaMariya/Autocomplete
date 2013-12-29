namespace Autocompletter
{
    public class TrieNode
    {
        public VocabularyItem VocabularyItem { get; set; }
        public TrieNode[] NextSymbols { get; set; }

        public TrieNode()
        {
            NextSymbols = new TrieNode[Configuration.Default.VariousSymbolsCount];
        }
        
        public TrieNode this[char symbol]
        {
            get
            {
                return NextSymbols[symbol - Configuration.Default.MinSymbolCode];
            }

            set
            {
                NextSymbols[symbol - Configuration.Default.MinSymbolCode] = value;
            }
        }

        public bool TryGetValue(string sequence, out TrieNode value)
        {
            TrieNode curNode = this;

            foreach (char symbol in sequence)
            {
                curNode = curNode[symbol];

                if (curNode == null)
                {
                    value = null;
                    return false;
                }
            }

            value = curNode;
            return true;
        }
    }
}
