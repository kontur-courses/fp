using System.Collections.Generic;
using TagsCloud.Words;

namespace TagsCloud.Tests
{
    public class ConstWordCollection : IBoringWordsCollection
    {
        private readonly List<string> words;

        public ConstWordCollection(List<string> words)
        {
            this.words = words;
        }

        public Result<List<string>> DeleteBoringWords()
        {
            return words;
        }
    }
}