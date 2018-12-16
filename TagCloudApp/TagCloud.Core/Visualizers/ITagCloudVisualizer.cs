using System.Collections.Generic;
using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Visualizers
{
    public interface ITagCloudVisualizer
    {
        Result<Bitmap> Render(IEnumerable<TagStat> tagStats);
    }
}