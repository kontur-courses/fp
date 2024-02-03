using System.Drawing.Text;
using ResultOf;
using TagCloud.AppSettings;

namespace TagCloud;

public static class SettingsValidator
{
    public static Result<IAppSettings> FontIsInstalled(IAppSettings settings)
    {
        using (var ifc = new InstalledFontCollection())
        {
            return ifc.Families.Any(f => f.Name == settings.FontType)
                ? Result.Ok(settings)
                : Result.Fail<IAppSettings>($"Font {settings.FontType} doesn't installed");
        }
    }

    public static Result<IAppSettings> SizeIsValid(IAppSettings settings)
    {
        return settings.CloudHeight > 0 && settings.CloudWidth > 0
            ? Result.Ok(settings)
            : Result.Fail<IAppSettings>("Image size params should be positive");
    }
}