using System.Drawing;
using System.Drawing.Imaging;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public class EmfImageSaver : StandardImageSaver
{
    public EmfImageSaver(IImagePathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
        
    }

    public override string SupportedExtension => ".emf";

    protected override void InternalSaveImage(Image image, string path)
    {
        image.Save(path, ImageFormat.Emf);
    }
}