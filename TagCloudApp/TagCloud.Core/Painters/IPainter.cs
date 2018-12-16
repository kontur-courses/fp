using System.Collections.Generic;
using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Painters
{
    public interface IPainter
    {
        Result<None> PaintTags(IEnumerable<Tag> tags);
        Result<None> SetBackgroundColorFor(Graphics graphics);
    }
}