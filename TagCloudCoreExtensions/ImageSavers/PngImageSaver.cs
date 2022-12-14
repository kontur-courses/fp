using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class PngImageSaver : IImageSaver
{
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    public PngImageSaver(IImagePathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public string SupportedExtension => ".png";

    public void SaveImage(Image image)
    {
        image.Save(_pathSettingsProvider.GetImagePathSettings().ImagePath, ImageFormat.Png);
    }
}