using System.Drawing;
using TagCloud.AppSettings;

namespace TagCloud.Drawer;

public class CustomPalette : IPalette
{
    public string Name => "Custom";

    public Color ForegroundColor { get; }
    public Color BackgroundColor { get; }

    public CustomPalette(IAppSettings settings)
    {
        ForegroundColor = Color.FromName(settings.ForegroundColor);
        BackgroundColor = Color.FromName(settings.BackgroundColor);
    }
}