using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.Interfaces
{
    public interface IPaintingSettings
    {
        Color BackgroundColor { get; }
        Brush TagBrush { get; }
    }
}