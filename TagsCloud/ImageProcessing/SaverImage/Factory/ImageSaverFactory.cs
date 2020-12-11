using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ImageProcessing.Config;
using TagsCloud.ImageProcessing.SaverImage.ImageSavers;
using TagsCloud.ResultOf;

namespace TagsCloud.ImageProcessing.SaverImage.Factory
{
    public class ImageSaverFactory : ServiceFactory<IImageSaver>
    {
        private readonly ImageConfig imageConfig;

        public ImageSaverFactory(ImageConfig imageConfig)
        {
            this.imageConfig = imageConfig;
        }

        public override Result<IImageSaver> Create()
        {
            var saver = services.FirstOrDefault(pair => pair.Value().CanSave(imageConfig.Path)).Value;

            return Result.Of(saver, $"This image type {imageConfig.Path.Split('.').Last()} is not supported");
        }
    }
}
