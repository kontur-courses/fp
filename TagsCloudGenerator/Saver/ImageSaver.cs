using System.Drawing;
using TagsCloudGenerator.Visualizer;

namespace TagsCloudGenerator.Saver
{
    public class ImageSaver : IImageSaver
    {
        private readonly string outputPath;
        private readonly ImageSettings imageSettings;

        public ImageSaver(string outputPath, ImageSettings imageSettings)
        {
            this.outputPath = outputPath;
            this.imageSettings = imageSettings;
        }

        public Result<None> Save(Bitmap bitmap)
        {
            return Result.OfAction(() => bitmap.Save(outputPath, imageSettings.ImageFormat));
        }
    }
}