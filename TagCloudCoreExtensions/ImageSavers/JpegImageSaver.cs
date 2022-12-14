using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class JpegImageSaver : IImageSaver
{
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    public JpegImageSaver(IImagePathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public string SupportedExtension => ".jpeg";

    public void SaveImage(Image image)
    {
        image.Save(_pathSettingsProvider.GetImagePathSettings().ImagePath, ImageFormat.Jpeg);
    }
}