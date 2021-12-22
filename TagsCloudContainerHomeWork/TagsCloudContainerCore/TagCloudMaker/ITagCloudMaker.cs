using System.Collections.Generic;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.TagCloudMaker;

public interface ITagCloudMaker
{
    // ReSharper disable once UnusedMember.Global
    public Result<IEnumerable<TagToRender>> GetTagsToRender(IEnumerable<string> tags);
}