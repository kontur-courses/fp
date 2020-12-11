using System;
using System.IO;
using ResultOf;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImagePathSettingManager : ISettingsManager
    {
        private readonly Func<IImageSettingsProvider> imageSettingsProvider;

        public ImagePathSettingManager(Func<IImageSettingsProvider> imageSettingsProvider)
        {
            this.imageSettingsProvider = imageSettingsProvider;
        }

        public string Title => "Image path";
        public string Help => "Type image file location to save";

        public Result<string> TrySet(string path)
        {
            path = Path.GetFullPath(path);
            imageSettingsProvider().ImagePath = path;
            return File.Exists(path)
                ? Result.Fail<string>($"File {path} already exists and will be overwritten!")
                : Get();
        }

        public string Get()
        {
            return imageSettingsProvider().ImagePath;
        }
    }
}