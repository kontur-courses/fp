using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.Preprocessing
{
    public class BoringWordsExcluder : IWordsPreprocessor
    {
        private readonly WordsPreprocessorSettings settings;

        public BoringWordsExcluder(WordsPreprocessorSettings settings)
        {
            this.settings = settings;
        }

        public OperationResult<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return new OperationResult<IEnumerable<string>>(ExcludeBoringWords(words), null);
        }

        private IEnumerable<string> ExcludeBoringWords(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower())
                .Where(word => word.Length > settings.BoringWordsLength && !settings.ExcludedWords.Contains(word));
        }
    }
}