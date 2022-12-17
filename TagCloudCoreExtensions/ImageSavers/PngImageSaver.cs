using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class PngImageSaver : StandardImageSaver
{
    public PngImageSaver(IImagePathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
        
    }

    public override string SupportedExtension => ".png";

    protected override void InternalSaveImage(Image image, string path)
    {
        image.Save(path, ImageFormat.Png);
    }
}