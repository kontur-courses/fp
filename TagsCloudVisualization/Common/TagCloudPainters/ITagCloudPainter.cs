using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.Tags;

namespace TagsCloudVisualization.Common.TagCloudPainters
{
    public interface ITagCloudPainter
    {
        public Result<Bitmap> Paint(IEnumerable<Tag> tags);
    }
}