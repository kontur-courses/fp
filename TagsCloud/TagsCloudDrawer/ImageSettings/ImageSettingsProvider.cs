using System.Drawing;
using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudDrawer.ImageSettings
{
    public class ImageSettingsProvider : IImageSettingsProvider
    {
        public static ImageSettingsProvider Default { get; } = new(Color.Gray, new Size(800, 600));

        public Color BackgroundColor { get; }

        public Size ImageSize { get; }

        private ImageSettingsProvider(Color backgroundColor, Size imageSize)
        {
            BackgroundColor = backgroundColor;
            ImageSize = imageSize;
        }

        public static Result<ImageSettingsProvider> Create(Color backgroundColor, PositiveSize imageSize)
        {
            return Result.Ok()
                .ToValue(new ImageSettingsProvider(backgroundColor, imageSize));
        }
    }
}