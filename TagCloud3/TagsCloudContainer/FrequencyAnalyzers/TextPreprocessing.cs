using ResultOf;
using TagsCloudContainer.TextTools;

namespace TagsCloudContainer.FrequencyAnalyzers
{
    public class TextPreprocessing
    {
        private readonly string excludedWordsPath;

        public TextPreprocessing(string excludedWordsPath = null)
        {
            this.excludedWordsPath = excludedWordsPath;
        }

        public Result<IEnumerable<string>> Preprocess(string text)
        {
            HashSet<string> excludedWords = new();

            if (excludedWordsPath != null)
            {
                var readExclude = TextFileReader.ReadText(excludedWordsPath);

                if (readExclude.IsSuccess)
                {
                    excludedWords = readExclude
                        .GetValueOrDefault(string.Empty)
                        .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToHashSet();
                }
                else
                {
                    return Result<IEnumerable<string>>.Fail(readExclude.Error);
                }
            }

            var words = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower());

            return Result<IEnumerable<string>>.Ok(FilterWords(words, excludedWords));
        }

        private IEnumerable<string> FilterWords(IEnumerable<string> words, HashSet<string> exclude)
        {
            foreach (var word in words)
            {
                if (!exclude.Contains(word))
                    yield return word;
            }
        }
    }
}