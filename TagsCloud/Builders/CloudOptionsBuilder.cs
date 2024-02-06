using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Builders;

public class CloudOptionsBuilder
{
    private ColoringStrategy coloringStrategy;
    private Color[] colors;
    private FontFamily fontFamily;
    private ILayout layout;
    private int maxFontSize;
    private MeasurerType measurerType;
    private int minFontSize;
    private SortType sortType;

    public Result<CloudOptionsBuilder> SetColoringStrategy(ColoringStrategy strategy)
    {
        coloringStrategy = strategy;
        return this;
    }

    public Result<CloudOptionsBuilder> SetMeasurerType(MeasurerType type)
    {
        measurerType = type;
        return this;
    }

    public Result<CloudOptionsBuilder> SetColors(HashSet<string> hexes)
    {
        var colorsSet = new HashSet<Color>();

        foreach (var hex in hexes)
            if (Color.TryParseHex(hex, out var color))
                colorsSet.Add(color);

        colors = colorsSet.Count == 0 ? new[] { Color.Black } : colorsSet.ToArray();
        return this;
    }

    public Result<CloudOptionsBuilder> SetFontBounds(int lowerBound, int upperBound)
    {
        if (lowerBound <= 0 || upperBound <= 0)
            return ResultExtensions.Fail<CloudOptionsBuilder>(
                $"Font bounds can't be <= 0! LowerBound = {lowerBound}, UpperBound = {upperBound}");

        if (lowerBound >= upperBound)
            return ResultExtensions.Fail<CloudOptionsBuilder>(
                $"LowerBound must be < UpperBound! LowerBound = {lowerBound}, UpperBound = {upperBound}");

        minFontSize = lowerBound;
        maxFontSize = upperBound;

        return this;
    }

    public Result<CloudOptionsBuilder> SetSortingType(SortType type)
    {
        sortType = type;
        return this;
    }

    public Result<CloudOptionsBuilder> SetFontFamily(string fontPath)
    {
        return fontPath.Contains(".ttf") ? TryLoadFontByFile(fontPath) : TryLoadFontByName(fontPath);
    }

    public Result<CloudOptionsBuilder> SetLayout(
        PointGeneratorType generator,
        PointF center,
        float distanceDelta,
        float angleDelta)
    {
        var pointGenerator = generator switch
        {
            PointGeneratorType.Spiral => new SpiralPointGenerator(distanceDelta, angleDelta),
            _ => new SpiralPointGenerator(distanceDelta, angleDelta)
        };

        layout = new Layout(pointGenerator, center);
        return this;
    }

    public Result<ICloudProcessorOptions> BuildOptions()
    {
        return new CloudProcessorOptions
        {
            ColoringStrategy = coloringStrategy,
            FontFamily = fontFamily,
            MaxFontSize = maxFontSize,
            MinFontSize = minFontSize,
            MeasurerType = measurerType,
            Sort = sortType,
            Layout = layout,
            Colors = colors
        };
    }

    private Result<CloudOptionsBuilder> TryLoadFontByName(string fontPath)
    {
        var fontCollection = new FontCollection();
        fontCollection.AddSystemFonts();

        var families = fontCollection.Families.ToArray();
        fontFamily = families
            .FirstOrDefault(family => family.Name.Equals(fontPath, StringComparison.OrdinalIgnoreCase));

        if (fontFamily != default)
            return this;

        var candidates = string.Join(", ", families.Select(family => family.Name));
        return ResultExtensions.Fail<CloudOptionsBuilder>(
            $"{fontPath} family is unknown! Candidates are: {candidates}");
    }

    private Result<CloudOptionsBuilder> TryLoadFontByFile(string fontPath)
    {
        var fontCollection = new FontCollection();

        if (!File.Exists(fontPath))
            return ResultExtensions.Fail<CloudOptionsBuilder>($"{fontPath} font file not found!");

        fontCollection.Add(fontPath);
        fontFamily = fontCollection.Families.First();

        return this;
    }
}