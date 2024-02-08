using System.Drawing;


// Changes opacity of color based on font size
public class OpacityColorProvider : IColorProvider
{
    private readonly RenderOptions options;

    public OpacityColorProvider(RenderOptions options)
    {
        this.options = options;
    }

    public Result<Color> GetColor(WordLayout layout)
    {
        var baseC = options.ColorScheme.FontColor;
        var baseOpacity = 0.4;
        var addOpacity = 1 - baseOpacity;

        var opacity = TagCloudHelpers.GetMultiplier(layout.FontSize, options.MinFontSize, options.MaxFontSize)
                                     .Then(mul => 255 * baseOpacity + 255 * addOpacity * mul);

        if (!opacity.IsSuccess)
            return Result.Fail<Color>($"Can't calculate color for word {layout}");

        var newColor = Color.FromArgb((int)opacity.Value, baseC.R, baseC.G, baseC.B);

        return newColor;
    }
}

