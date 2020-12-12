using ResultOf;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainer.App.TextAnalyzer
{
    internal class ToLowerWordNormalizer : IWordNormalizer
    {
        public Result<string> NormalizeWord(string word)
        {
            return Result.Of(() => word.ToLower());
        }
    }
}