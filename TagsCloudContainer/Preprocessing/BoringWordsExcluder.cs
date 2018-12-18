using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Preprocessing
{
    public class BoringWordsExcluder : IWordsPreprocessor
    {
        private readonly WordsPreprocessorSettings settings;

        public BoringWordsExcluder(WordsPreprocessorSettings settings)
        {
            this.settings = settings;
        }

        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return Result.Of(() => ExcludeBoringWords(words));
        }

        private IEnumerable<string> ExcludeBoringWords(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower())
                .Where(word => word.Length > settings.BoringWordsLength && !settings.ExcludedWords.Contains(word));
        }
    }
}