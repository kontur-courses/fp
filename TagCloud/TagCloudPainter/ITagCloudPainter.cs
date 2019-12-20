using System.Collections.Generic;
using ResultLogic;
using TagCloud.TextPreprocessor.Core;

namespace TagCloud.TagCloudPainter
{
    public interface ITagCloudPainter
    {
        Result<None> Draw(IEnumerable<TagInfo> tagInfos);
    }
}