using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class EmfImageSaver : IImageSaver
{
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    public EmfImageSaver(IImagePathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public string SupportedExtension => ".emf";

    public void SaveImage(Image image)
    {
        image.Save(_pathSettingsProvider.GetImagePathSettings().ImagePath, ImageFormat.Emf);
    }
}