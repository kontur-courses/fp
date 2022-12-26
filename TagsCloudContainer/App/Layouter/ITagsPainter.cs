using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public interface ITagsPainter
    {
        public Result<None> Paint(IEnumerable<TagInfo> tags, Size imageSize, Graphics graphics);
    }
}


