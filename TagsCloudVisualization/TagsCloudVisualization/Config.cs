using System.Drawing;

namespace TagsCloudVisualization;

public static class Config
{
    public const int
        WindowWidth = 800,
        WindowHeight = 600,
        CenterX = 300,
        CenterY = 200;

    public const string
        DefaultPath = "..\\..\\..\\input.txt";

    public const string DefaultFont = "Arial";
    public const int DefaultFontSize = 16;

    public static Color DefaultTextColor => Color.Red;
}