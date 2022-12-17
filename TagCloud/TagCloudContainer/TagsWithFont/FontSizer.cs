using TagCloudContainer.FrequencyWords;

namespace TagCloudContainer.TagsWithFont
{
    public class FontSizer : IFontSizer
    {
        public Result<IEnumerable<FontTag>> GetTagsWithSize(IEnumerable<WordFrequency> tags, IFontSettings settings)
        {
            Result.OfAction(() => CheckSettings(settings)).GetValueOrThrow();
            return Result.Of(() => (from tag in tags
                let size = (int)Math.Round(tag.Count == tags.Last().Count
                    ? (int)Math.Round((double)settings.MinFont)
                    : tag.Count / (double)tags.First().Count * (settings.MaxFont - settings.MinFont) + settings.MinFont)
                select new FontTag(tag.Word, size, settings.Font)).ToList()).Then(x=>x as IEnumerable<FontTag>);
        }

        private static void CheckSettings(IFontSettings settings)
        {
            if (settings.MaxFont <= 0 || settings.MinFont <= 0)
                throw new ArgumentNullException("sizeAvgTagSize must be > 0");
            if (settings.MinFont >= settings.MaxFont)
                throw new ArgumentNullException("fontMax must be larger than fontMin");
        }
    }
}
