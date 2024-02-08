using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;

namespace TagsCloudContainer.Settings;

public interface IImageSettings: ISettings
{
    public TextOptions TextOptions { get; set; }

    public Size ImageSize { get; set; }
    
    public Color PrimaryColor { get; set; }

    public Color BackgroundColor { get; set; }
}