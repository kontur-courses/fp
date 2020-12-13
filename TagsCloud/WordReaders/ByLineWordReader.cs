using System;
using System.Collections.Generic;
using System.IO;
using TagsCloud.WordSelectors;

namespace TagsCloud.WordReaders
{
    public class ByLineWordReader : IWordReader
    {
        private readonly string filePath;
        private readonly IWordSelector selector;
        private readonly string[] separator = {Environment.NewLine};

        public ByLineWordReader(string filePath, IWordSelector selector)
        {
            this.filePath = filePath;
            this.selector = selector;
        }

        public IEnumerable<string> ReadWords()
        {
            var words = File.ReadAllLines(filePath);
            return selector.TakeSelectedWords(words);
        }
    }
}