using System.Drawing;
using System.IO;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.ImagesSavers
{
    public class ImageSaver : IImageSaver
    {
        private readonly SaveSettings settings;

        public ImageSaver(SaveSettings settings) => this.settings = settings;

        public Result<Image> Save(Image image)
        {
            if (!Directory.Exists(settings.OutputDirectory))
                return Result.Fail<Image>($"directory {settings.OutputDirectory} not found");

            var path = Path.Combine(settings.OutputDirectory, $"{settings.OutputFileName}.{settings.Extension}");
            image.Save(path);
            return image;
        }
    }
}