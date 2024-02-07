using System.Drawing;

namespace TagCloud.Drawer;

public interface IPalette
{
    string Name { get; }
    Color ForegroundColor { get; }
    Color BackgroundColor { get; }
}