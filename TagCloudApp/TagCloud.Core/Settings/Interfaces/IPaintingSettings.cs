using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.Interfaces
{
    public interface IPaintingSettings
    {
        Result<Color> BackgroundColorResult { get; }
        Result<Brush> TagBrushResult { get; }
    }
}