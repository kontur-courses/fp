using System.Drawing;
using System.IO;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.ImagesSavior
{
    public class ImageSavior : IImageSavior
    {
        private readonly SaveSettings settings;

        public ImageSavior(SaveSettings settings) => this.settings = settings;

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