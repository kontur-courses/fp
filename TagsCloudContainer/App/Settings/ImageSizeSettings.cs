using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class ImageSizeSettings : IImageSizeSettingsHolder
    {
        public static readonly int DefaultWidth = 500;
        public static readonly int DefaultHeight = 500;

        public static readonly ImageSizeSettings Instance = new ImageSizeSettings();

        private ImageSizeSettings()
        {
            SetDefault();
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public void SetDefault()
        {
            Width = DefaultWidth;
            Height = DefaultHeight;
        }
    }
}