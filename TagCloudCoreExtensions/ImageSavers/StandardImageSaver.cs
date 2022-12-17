using System.Drawing;
using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.ImageSavers;

public abstract class StandardImageSaver : IImageSaver
{
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    protected StandardImageSaver(IImagePathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public abstract string SupportedExtension { get; }

    public Result<None> SaveImage(Image image)
    {
        var path = _pathSettingsProvider.GetImagePathSettings().ImagePath;
        var ext = Path.GetExtension(path);
        return ext == SupportedExtension
            ? Result.OfAction(() => InternalSaveImage(image, path))
            : Result.Fail<None>($"Wrong file extension: {ext}");
    }

    protected abstract void InternalSaveImage(Image image, string path);
}