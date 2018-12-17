using System.Drawing;

namespace TagsCloudResult.Settings
{
    public class DefaultFontSettings : IFontSettings
    {
        public FontFamily FontFamily { get; } = new FontFamily("Times New Roman");
        public Brush Brush { get; } = Brushes.Black;
    }
}