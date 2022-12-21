using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationConsole;

public static class VisualizationOptionsConverter
{
    public static Result<VisualizationOptions> ConvertOptions(ArgsOptions argsOptions)
    {
        var visualizationsOptions = new VisualizationOptions();

        if (!Directory.Exists(argsOptions.DirectoryToMyStemProgram))
            return Result.Fail<VisualizationOptions>($"Check argsOptions. Directory for mystem not exist {argsOptions.DirectoryToMyStemProgram}.");

        if (argsOptions.CanvasWidth < 1 || argsOptions.CanvasHeight < 1)
            return Result.Fail<VisualizationOptions>("Check argsOptions. Canvas width and height must be greater or equals than 1.");

        var backgroundColor = Color.FromName(argsOptions.BackgroundColor);
        if (!backgroundColor.IsKnownColor)
            return Result.Fail<VisualizationOptions>($"Check argsOptions. Color with name {argsOptions.BackgroundColor} is not valid.");

        if (string.IsNullOrEmpty(argsOptions.FontFamily))
            return Result.Fail<VisualizationOptions>($"Check argsOptions. FontFamily can't be null.");

        var fontFamily = new FontFamily(argsOptions.FontFamily);
        if (fontFamily.Name != argsOptions.FontFamily)
            return Result.Fail<VisualizationOptions>($"Check argsOptions. Font \"{argsOptions.FontFamily}\" can't be found in system.");

        if (argsOptions.MinFontSize < 1)
            return Result.Fail<VisualizationOptions>("Check argsOptions. MinFontSize must be greater or equals than 1.");

        if (argsOptions.MaxFontSize <= argsOptions.MinFontSize)
            return Result.Fail<VisualizationOptions>("Check argsOptions. MaxFontSize  must be greater than MinFontSize.");

        var defaultPaletteBrush = Color.FromName(argsOptions.PaletteDefaultBrush);
        if (!defaultPaletteBrush.IsKnownColor)
            return Result.Fail<VisualizationOptions>($"Check argsOptions. Color {argsOptions.PaletteDefaultBrush} is not valid fore default palette brush.");
        var palette = new Palette(new SolidBrush(defaultPaletteBrush));

        foreach (var colorName in argsOptions.PaletteAvailableBrushes)
        {
            var color = Color.FromName(colorName);
            if (!color.IsKnownColor)
                return Result.Fail<VisualizationOptions>($"Check argsOptions. Invalid color {color} for palette.");
            palette.AvailableBrushes!.Add(new SolidBrush(color));
        }

        visualizationsOptions.CanvasSize = new Size(argsOptions.CanvasWidth, argsOptions.CanvasHeight);
        visualizationsOptions.SpiralStep = argsOptions.SpiralStep;
        visualizationsOptions.BackgroundColor = backgroundColor;
        visualizationsOptions.TakeMostPopularWords = argsOptions.TakeMostPopularWords;
        visualizationsOptions.BoringWords = argsOptions.BoringWords;
        visualizationsOptions.ExcludedPartsOfSpeech = argsOptions.ExcludedPartsOfSpeech;
        visualizationsOptions.FontFamily = fontFamily;
        visualizationsOptions.MinFontSize = argsOptions.MinFontSize;
        visualizationsOptions.MaxFontSize = argsOptions.MaxFontSize;
        visualizationsOptions.Palette = palette;
        visualizationsOptions.DirectoryToMyStemProgram = argsOptions.DirectoryToMyStemProgram;

        return visualizationsOptions;
    }
}