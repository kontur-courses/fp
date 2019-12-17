using System.Drawing;

namespace TagsCloudVisualization.Services
{
    public struct ImageSettings
    {
        private static readonly ImageSettings DefaultSettings = InitializeDefaultSettings();
        public  Font Font { get; set; }
        public  Size ImageSize { get; set; }
        public  Color BackgroundColor { get; set; }
        public int MaximumTagsToDraw { get; set; }

        public ImageSettings(Font font, Size imageSize, Color backgroundColor, int maximumTagsToDraw)
        {
            BackgroundColor = backgroundColor;
            Font = font ?? DefaultSettings.Font;
            ImageSize = imageSize;
            MaximumTagsToDraw = maximumTagsToDraw;
        }

        public static ImageSettings InitializeDefaultSettings()
        {
            var font = new Font(FontFamily.GenericSansSerif, 30, FontStyle.Bold);
            return new ImageSettings(font, new Size(1000, 1000), Color.AntiqueWhite, 1000);
        }
    }
}