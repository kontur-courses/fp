using System.Drawing;
using TagCloud.ExceptionHandler;
using TagCloud.Visualizer;

namespace TagCloud.ImageSaver
{
    public interface IImageSaver
    {
        public Result<bool> TrySaveImage(Result<Bitmap> bitmapResult, string savePath, ImageOptions imageOptions);
    }
}