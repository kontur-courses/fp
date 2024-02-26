using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer;

public class Settings
{
    public string FontName { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
    public string File { get; set; }
    public string BoringWordsFileName { get; set; }
    public int CenterX { get; set; }
    public int CenterY { get; set; }
    public string ImageFormat { get; set; }
}
