using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagsCloud.WordSelectors;

namespace TagsCloud.WordReaders
{
    public class RegexWordReader : IWordReader
    {
        private const string SplitPattern = "\\W+";
        
        private readonly string filePath;
        private readonly IWordSelector selector;

        public RegexWordReader(string filePath, IWordSelector selector)
        {
            this.filePath = filePath;
            this.selector = selector;
        }

        public IEnumerable<string> ReadWords()
        {
            var file = File.ReadAllText(filePath);
            var words = Regex.Split(file, SplitPattern);
            return selector.TakeSelectedWords(words);
        }
    }
}