using TagCloudContainer.FrequencyWords;
using TagCloudContainer.Models;
using TagCloudContainer.Result;

namespace TagCloudContainer.Interfaces
{
    public interface IFontSizer
    {
       Result<IEnumerable<ITag>> GetTagsWithSize(IEnumerable<TagWithFrequency> tags, IFontSettings settings);
    }
}
