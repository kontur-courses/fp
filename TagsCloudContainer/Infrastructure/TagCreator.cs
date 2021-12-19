using TagsCloudContainer.Extensions;
using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainer.Infrastructure
{
    public class TagCreator : ITagCreator
    {
        private Dictionary<string, int> wordStatistics = new();
        private int wordCount;

        public Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words)
        {
            wordStatistics = new Dictionary<string, int>();
            CalculateStatistics(words);

            if (wordCount < 1)
                return Result.Fail<IEnumerable<Tag>>("No tags created!");

            var result = wordStatistics
                .Select(statistic
                => new Tag(
                    (double)statistic.Value / wordCount, statistic.Key));
            return Result.Ok(result);
        }

        private void CalculateStatistics(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                wordStatistics.Increment(word);
                wordCount++;
            }
        }
    }
}
