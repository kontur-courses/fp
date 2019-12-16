using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TagsCloudResult.CloudVisualizers.ImageSaving
{
    public class ImageSaver : IImageSaver
    {
        private readonly Func<ImageSaverSettings> settingsFactory;

        public ImageSaver(Func<ImageSaverSettings> settingsFactory)
        {
            this.settingsFactory = settingsFactory;
        }

        public Result<None> Save(Bitmap bitmap)
        {
            var settings = settingsFactory();
            try
            {
                bitmap.Save(settings.Path, settings.Format);
            }
            catch (ArgumentNullException)
            {
                return Result.Fail<None>(settings.Path is null
                    ? "Saving image path is null."
                    : "Saving image format is null.");
            }
            catch (ExternalException)
            {
                return Result.Fail<None>("Image is saved in wrong format.");
            }
            return Result.Ok();
        }
    }
}