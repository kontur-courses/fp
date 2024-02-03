using ResultOf;

namespace TagCloud.Drawer;

public interface IPaletteProvider
{
    Result<IPalette> CreatePalette(string paletteName);
}