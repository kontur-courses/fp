using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
