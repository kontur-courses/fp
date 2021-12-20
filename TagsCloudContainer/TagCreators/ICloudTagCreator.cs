using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagCreators;

public interface ICloudTagCreator
{
    Result<IEnumerable<CloudTag>> CreateCloudTags(IEnumerable<PaintedTag> tags);
}
