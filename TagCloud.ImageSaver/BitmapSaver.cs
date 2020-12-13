using System.Drawing;
using System.IO;
using TagCloud.ExceptionHandler;
using TagCloud.Visualizer;

namespace TagCloud.ImageSaver
{
    public class BitmapSaver : IImageSaver
    {
        public Result<bool> TrySaveImage(Result<Bitmap> bitmapResult, string savePath, ImageOptions imageOptions)
        {
            if (Path.GetExtension(imageOptions.ImageSaveName) != ".bmp")
            {
                return false;
            }

            bitmapResult.Save(Path.Combine(savePath, $"{imageOptions.ImageSaveName}"));
            return true;
        }
    }
}