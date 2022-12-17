using TagCloudContainer.FrequencyWords;

namespace TagCloudContainer.TagsWithFont
{
    public interface IFontSizer
    {
        Result<IEnumerable<FontTag>> GetTagsWithSize(IEnumerable<WordFrequency> tags, IFontSettings settings);
    }
}
