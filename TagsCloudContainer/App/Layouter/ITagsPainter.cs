using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public interface ITagsPainter
    {
        public Result<None> Paint(IEnumerable<TagInfo> tags);
    }
}


