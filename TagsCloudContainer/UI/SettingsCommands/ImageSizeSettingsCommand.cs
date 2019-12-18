using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudContainer.UI.SettingsCommands
{
    public class ImageSizeSettingsCommand : SettingsCommand
    {
        public override string Name { get; } = "size";

        public override Result<IInitialSettings> TryChangeSettings(string[] arguments, IInitialSettings settings)
        {
            if (arguments.Length <= 1 || arguments[1] == "auto")
                return GetSettingsResult(settings, Size.Empty);
            if (arguments.Length < 3)
                return Result.Fail<IInitialSettings>("Should be 2 arguments: width and height");
            if (!int.TryParse(arguments[1], out var width) || !int.TryParse(arguments[2], out var height))
                return Result.Fail<IInitialSettings>("Arguments should be integers");
            if (width <= 0 || height <= 0)
                return Result.Fail<IInitialSettings>("Width and height should be positive");
            return GetSettingsResult(settings, new Size(width, height));
        }

        private Result<IInitialSettings> GetSettingsResult(IInitialSettings settings, Size size)
        {
            var newSettings = (IInitialSettings)settings.Clone();
            newSettings.ImageSize = size;
            return Result.Ok(newSettings);
        }
    }
}
