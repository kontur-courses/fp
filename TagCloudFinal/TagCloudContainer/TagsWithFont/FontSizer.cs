using TagCloudContainer.FrequencyWords;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Models;
using TagCloudContainer.Result;

namespace TagCloudContainer.TagsWithFont
{
    public class FontSizer : IFontSizer
    {
        public Result<IEnumerable<ITag>> GetTagsWithSize(IEnumerable<TagWithFrequency> tags, IFontSettings settings)
        {
            var sizeList = new List<ITag>();

            foreach (var tag in tags)
            {
                var size = (int)Math.Round(tag.Count == tags.Last().Count
                    ? (int)Math.Round((double)settings.MinFontSize)
                    : tag.Count / (double)tags.First().Count * (settings.MaxFontSize - settings.MinFontSize) +
                      settings.MinFontSize);

                sizeList.Add(new TagWithFont(tag.Word, size, settings.Font));
            }

            return Result.Result.Ok(sizeList.Select(x => x));
        }
    }
}
