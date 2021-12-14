using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class TagCreator : ITagCreator
    {
        private Dictionary<string, int> wordStatistics = new();
        private int wordCount = 0;

        public TagCreator()
        {
        }

        public Result<IEnumerable<Tag>> CreateTags(IEnumerable<string> words)
        {
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
