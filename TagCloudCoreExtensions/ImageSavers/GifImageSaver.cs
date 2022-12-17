using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class GifImageSaver : StandardImageSaver
{
    public GifImageSaver(IImagePathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
        
    }

    public override string SupportedExtension => ".gif";

    protected override void InternalSaveImage(Image image, string path)
    {
        image.Save(path, ImageFormat.Gif);
    }
}