using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class JpegImageSaver : StandardImageSaver
{
    public JpegImageSaver(IImagePathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
        
    }

    public override string SupportedExtension => ".jpeg";

    protected override void InternalSaveImage(Image image, string path)
    {
        image.Save(path, ImageFormat.Jpeg);
    }
}