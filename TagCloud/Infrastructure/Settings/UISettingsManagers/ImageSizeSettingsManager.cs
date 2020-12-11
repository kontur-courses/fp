using System;
using System.Text.RegularExpressions;
using ResultOf;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImageSizeSettingsManager : ISettingsManager
    {
        private readonly Func<IImageSettingsProvider> imageSettingsProvider;
        private readonly Regex regex;

        public ImageSizeSettingsManager(Func<IImageSettingsProvider> imageSettingsProvider)
        {
            this.imageSettingsProvider = imageSettingsProvider;
            regex = new Regex(@"^(?<width>\d+)\s+(?<height>\d+)$");
        }

        public string Title => "Size";
        public string Help => "Input image width and height separated by space";

        public Result<string> TrySet(string input)
        {
            var match = regex.Match(input);
            if (!match.Success)
                return Result.Fail<string>("Incorrect input format ([width], [height])");
            imageSettingsProvider().Width = int.Parse(match.Groups["width"].Value);
            imageSettingsProvider().Height = int.Parse(match.Groups["height"].Value);
            return Get();
        }

        public string Get()
        {
            var settings = imageSettingsProvider();
            return $"{settings.Width} {settings.Height}";
        }
    }
}