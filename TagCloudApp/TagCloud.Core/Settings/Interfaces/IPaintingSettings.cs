using System.Drawing;

namespace TagCloud.Core.Settings.Interfaces
{
    public interface IPaintingSettings
    {
        Color BackgroundColor { get; }
        Brush TagBrush { get; }
    }
}