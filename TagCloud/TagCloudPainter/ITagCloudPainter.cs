using System.Collections.Generic;
using TagCloud.TextPreprocessor.Core;
using TagsCloud;

namespace TagCloud.TagCloudPainter
{
    public interface ITagCloudPainter
    {
        Result<None> Draw(IEnumerable<TagInfo> tagInfos);
    }
}