using ResultOf;
using TagsCloudContainer.Words;

namespace TagsCloudContainer.Cloud
{
    public interface ITagCloud
    {
        Result<WordTag[]> Tags { get; }
    }
}