using TagCloudContainer.Models;
using TagCloudContainer.Result;

namespace TagCloudContainer.Interfaces
{
    public interface IFrequencyCounter
    {
        Result<IEnumerable<TagWithFrequency>> GetTagsFrequency(IEnumerable<string> words);
    }
}
