using System.Drawing;

namespace TagsCloudResult.Settings
{
    public interface IFontSettings
    {
        FontFamily FontFamily { get; set; }
        Brush Brush { get; set; }
    }
}
