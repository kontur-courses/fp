using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.Interfaces
{
    public interface IVisualizingSettings
    {
        int Width { get; set; }
        int Height { get; set; }
        float MinFontSize { get; set; }
        float MaxFontSize { get; set; }

        PointF CenterPoint { get; }
        Result<Font> DefaultFontResult { get; }
    }
}