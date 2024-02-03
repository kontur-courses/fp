using ResultOf;

namespace TagCloud.Drawer;

public class PaletteProvider : IPaletteProvider
{
    private Dictionary<string, IPalette> palettes;

    public PaletteProvider(IEnumerable<IPalette> palettes)
    {
        this.palettes = ArrangePalettes(palettes);
    }

    public Result<IPalette> CreatePalette(string paletteName)
    {
        return palettes.ContainsKey(paletteName)
            ? Result.Ok(palettes[paletteName])
            : Result.Fail<IPalette>($"Palette with name {paletteName} doesn't exist");
    }

    private Dictionary<string, IPalette> ArrangePalettes(IEnumerable<IPalette> palettes)
    {
        var palettesDictionary = new Dictionary<string, IPalette>();
        foreach (var palette in palettes)
        {
            palettesDictionary[palette.Name] = palette;
        }

        return palettesDictionary;
    }
}