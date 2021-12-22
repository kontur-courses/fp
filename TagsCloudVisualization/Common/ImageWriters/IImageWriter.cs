using System.Drawing;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Common.ImageWriters
{
    public interface IImageWriter
    {
        public Result<None> Save(Bitmap bitmap, string path);
    }
}