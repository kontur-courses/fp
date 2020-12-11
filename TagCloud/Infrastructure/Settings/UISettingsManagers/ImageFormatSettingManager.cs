using System;
using System.Drawing.Imaging;
using System.Linq;
using ResultOf;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImageFormatSettingManager : ISettingsManager
    {
        private readonly Func<IImageFormatSettingProvider> settingsProvider;

        public ImageFormatSettingManager(Func<IImageFormatSettingProvider> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public string Title => "Image Format";
        public string Help => "Choose format: Bmp, Emf, Exif, Gif, Icon, Jpeg, Png, Tiff, Wmf";
        public Result<string> TrySet(string extension)
        {
            var propertyInfos = typeof(ImageFormat)
                .GetProperties();
     
            var newFormat = propertyInfos
                .Where(info => info.Name.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                .Select(info => info.GetValue(settingsProvider().Format))
                .Cast<ImageFormat>()
                .SingleOrDefault();
            
            if (newFormat == null)
                return Result.Fail<string>($"{extension} is not supported extension");
            settingsProvider().Format = newFormat;
            return Get();
        }

        public string Get()
        {
            return settingsProvider().Format.ToString();
        }
    }
}