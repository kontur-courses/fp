using CommandLine;

namespace TagCloud.AppSettings;

public class Settings : IAppSettings
{
    [Option('s', "sourceFile", Default = "text.txt", HelpText = "Path to file with words to visualize")]
    public string InputPath { get; set; }

    [Option('o', "outputPath", Default = "result", HelpText = "Path to output image file")]
    public string OutputPath { get; set; }

    [Option('e', "extensionImage", Default = "png", HelpText = "Output image file format")]
    public string ImageExtension { get; set; }

    [Option('f', "fontType", Default = "Arial", HelpText = "Font type of words")]
    public string FontType { get; set; }

    [Option('W', "width", Default = 1920, HelpText = "Width of cloud in pixels")]
    public int CloudWidth { get; set; }

    [Option('H', "height", Default = 1080, HelpText = "Height of cloud in pixels")]
    public int CloudHeight { get; set; }

    [Option('l', "layouter", Default = "Spiral", HelpText = "Cloud layouter algorithm")]
    public string LayouterType { get; set; }

    [Option('d', "density", Default = 1, HelpText = "Density of cloud, integer")]
    public int CloudDensity { get; set; }

    [Option('p', "imagePalette", Default = "Random", HelpText = "Image coloring method")]
    public string ImagePalette { get; set; }

    [Option("background", Default = "White", HelpText = "Bckground color name")]
    public string BackgroundColor { get; set; }

    [Option("foreground", Default = "Black", HelpText = "Foreground color name")]
    public string ForegroundColor { get; set; }
}