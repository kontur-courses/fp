using System.Collections.Generic;
using System.Drawing;
using ErrorHandler;
using TagsCloudVisualization.Logic;

namespace TagsCloudVisualization.Services
{
    public interface IImageGenerator
    {
        Result<Bitmap> CreateImage(IEnumerable<Tag> tags);
    }
}