using System.Collections.Generic;

namespace TagCloud.Provider
{
    public class WordProvider : IWordProvider
    {
        public IEnumerable<string> Words => words;
        private readonly List<string> words;

        public WordProvider()
        {
            words = new List<string>();
        }

        public void AddWords(IEnumerable<string> words)
        {
            this.words.AddRange(words);
        }
    }
}
