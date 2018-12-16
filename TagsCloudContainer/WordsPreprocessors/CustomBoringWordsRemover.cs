using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.WordsPreprocessors
{
    public class CustomBoringWordsRemover : IWordsPreprocessor
    {
        private HashSet<string> customBoringWords;

        IEnumerable<string> IWordsPreprocessor.Preprocess(IEnumerable<string> words)
        {
            return words.Where(word => !customBoringWords.Contains(word));
        }

        public IWordsPreprocessor WithConfig(Config config)
        {
            customBoringWords = new HashSet<string>(((ICustomWordsRemoverConfig)config).CustomBoringWords);

            return this;
        }
    }
}