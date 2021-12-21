using System.Collections.Generic;

namespace TagsCloudContainerCore.TagCloudMaker;

public interface ITagCloudMaker
{
    // ReSharper disable once UnusedMember.Global
    public IEnumerable<TagToRender> GetTagsToRender(IEnumerable<string> tags);
}