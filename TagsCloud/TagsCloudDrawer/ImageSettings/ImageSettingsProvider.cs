using System.Drawing;
using ResultMonad;

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

        public static Result<ImageSettingsProvider> Create(Color backgroundColor, Size imageSize)
        {
            return Result.Ok()
                .Validate(() => imageSize.Width > 0, "Expected width of image to be positive")
                .Validate(() => imageSize.Height > 0, "Expected height of image to be positive")
                .ToValue(new ImageSettingsProvider(backgroundColor, imageSize));
        }
    }
}