using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class GifImageSaver : IImageSaver
{
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    public GifImageSaver(IImagePathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public string SupportedExtension => ".gif";

    public void SaveImage(Image image)
    {
        image.Save(_pathSettingsProvider.GetImagePathSettings().ImagePath, ImageFormat.Gif);
    }
}