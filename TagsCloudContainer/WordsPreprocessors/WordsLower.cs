using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TagsCloudContainer.WordsPreprocessors
{
    public class WordsLower : IWordsPreprocessor
    {
        public IEnumerable<string> Preprocess(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower(CultureInfo.CurrentCulture));
        }

        public IWordsPreprocessor WithConfig(Config config)
        {
            return this;
        }
    }
}