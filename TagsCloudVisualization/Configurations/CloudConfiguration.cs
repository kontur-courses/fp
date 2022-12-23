using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudVisualization.Configurations
{
    public class CloudConfiguration : IDisposable
    {
        public static CloudConfiguration Default => new(
            new Point(750, 750),
            new Size(1500, 1500),
            Color.FromArgb(255, 0, 34, 43),
            Color.FromArgb(255, 217,92,6),
            new FontFamily("Arial")
        );
        
        public Point Center { get; }
        public Size ImageSize { get; }
        public Color BackgroundColor { get; }
        public Color PrimaryColor { get; }
        public FontFamily FontFamily { get; }

        private CloudConfiguration(Point center, Size imageSize, Color backgroundColor, Color primaryColor, FontFamily fontFamily)
        {
            ImageSize = imageSize;
            BackgroundColor = backgroundColor;
            PrimaryColor = primaryColor;
            FontFamily = fontFamily;
            Center = center;
        }
        
        public static CloudConfiguration Create(Point center, Size imageSize, Color backgroundColor, Color primaryColor, FontFamily fontFamily)
        {
            var cloudConfiguration = new CloudConfiguration(center, imageSize, backgroundColor, primaryColor, fontFamily);

            return cloudConfiguration.Validate().GetValueOrThrow();
        }

        public Result<CloudConfiguration> Validate()
        {
            if (ImageSize.Width <= 0 || ImageSize.Height <= 0)
                return Result.Fail<CloudConfiguration>("Incorrect image size");
            
            if (!IsFontInstalled(FontFamily.Name))
                return Result.Fail<CloudConfiguration>("A font with this name was not found in the system");

            return Result.Ok(this);
        }

        private bool IsFontInstalled(string fontName)
        {
            var fontsCollection = new InstalledFontCollection();
            return fontsCollection.Families.Any(x => x.Name == fontName);
        }

        public void Dispose()
        {
            FontFamily?.Dispose();
        }
    }
}