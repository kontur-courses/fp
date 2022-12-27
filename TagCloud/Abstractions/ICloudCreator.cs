using FluentResults;

namespace TagCloud.Abstractions;

public interface ICloudCreator
{
    Result<IEnumerable<IDrawableTag>> CreateTagCloud(IEnumerable<ITag> tags);
}