using System.Drawing;

namespace TagsCloudContainer.Settings.Interfaces
{
    public interface IRenderingSettings
    {
        Size? DesiredImageSize { get; set; }
        float Scale { get; set; }
        Brush Background { get; set; }
    }
}