using Microsoft.Extensions.DependencyInjection;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloud.CustomAttributes;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.FontMeasurers;
using TagsCloud.Options;
using TagsCloud.Painters;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Processors;

[Injection(ServiceLifetime.Singleton)]
public class CloudProcessor : ICloudProcessor
{
    private readonly ICloudProcessorOptions cloudOptions;
    private readonly IEnumerable<IFontMeasurer> fontMeasurers;
    private readonly IEnumerable<IPainter> painters;

    public CloudProcessor(
        ICloudProcessorOptions cloudOptions,
        IEnumerable<IPainter> painters,
        IEnumerable<IFontMeasurer> fontMeasurers)
    {
        this.cloudOptions = cloudOptions;
        this.fontMeasurers = fontMeasurers;
        this.painters = painters;
    }

    public Result<HashSet<WordTagGroup>> SetPositions(HashSet<WordTagGroup> wordGroups)
    {
        var layout = cloudOptions.Layout;
        var imageBounds = new SizeF(layout.Center.X * 2, layout.Center.Y * 2);

        foreach (var group in GetSortedGroups(wordGroups))
        {
            var fontRect = TextMeasurer
                .MeasureAdvance(
                    group.WordInfo.Text,
                    new TextOptions(group.VisualInfo.TextFont));

            var rectangle = layout.PutNextRectangle(new SizeF(fontRect.Width, fontRect.Height));

            if (IsRectOutOfBounds(rectangle, imageBounds))
                return ResultExtensions.Fail<HashSet<WordTagGroup>>(
                    "TagCloud is out of bounds! Please, increase image size or reduce words amount.");

            if (!fontRect.Width.IsEqualTo(rectangle.Width))
                group.VisualInfo.IsRotated = true;

            group.VisualInfo.BoundsRectangle = rectangle;
        }

        return wordGroups;
    }

    public Result<HashSet<WordTagGroup>> SetFonts(HashSet<WordTagGroup> wordGroups)
    {
        var (minFontSize, maxFontSize) = (cloudOptions.MinFontSize, cloudOptions.MaxFontSize);
        var (minFrequency, maxFrequency) = GetMinMaxValues(wordGroups);

        var measurer = FindFontMeasurer();

        foreach (var group in wordGroups)
        {
            var fontSize = measurer.GetFontSize(
                group.Count,
                minFrequency,
                maxFrequency,
                minFontSize,
                maxFontSize);

            group.VisualInfo.TextFont = cloudOptions.FontFamily.CreateFont(fontSize, FontStyle.Regular);
        }

        return wordGroups;
    }

    public Result<HashSet<WordTagGroup>> SetColors(HashSet<WordTagGroup> wordGroups)
    {
        return painters
               .Single(painter => painter.Strategy == cloudOptions.ColoringStrategy)
               .AsResult()
               .Then(painter => painter.Colorize(wordGroups, cloudOptions.Colors));
    }

    private IEnumerable<WordTagGroup> GetSortedGroups(IEnumerable<WordTagGroup> wordGroups)
    {
        var sortedGroups = cloudOptions.Sort switch
        {
            SortType.Ascending => wordGroups.OrderBy(group => group.Count),
            SortType.Descending => wordGroups.OrderByDescending(group => group.Count),
            _ => wordGroups
        };

        return sortedGroups.Select(group => group);
    }

    private IFontMeasurer FindFontMeasurer()
    {
        return fontMeasurers.Single(measurer => measurer.Type == cloudOptions.MeasurerType);
    }

    private static bool IsRectOutOfBounds(RectangleF rect, SizeF bounds)
    {
        var isOutOfLeftBound = rect.X < 0;
        var isOutOfRightBound = rect.X + rect.Width > bounds.Width;
        var isOutOfTopBound = rect.Y < 0;
        var isOutOfBottomBound = rect.Y + rect.Height > bounds.Height;

        return isOutOfLeftBound || isOutOfRightBound || isOutOfTopBound || isOutOfBottomBound;
    }

    private static (int minValue, int maxValue) GetMinMaxValues(IEnumerable<WordTagGroup> wordGroups)
    {
        var (minValue, maxValue) = (int.MaxValue, int.MinValue);

        foreach (var currentCount in wordGroups.Select(group => group.Count))
        {
            minValue = currentCount < minValue ? currentCount : minValue;
            maxValue = currentCount > maxValue ? currentCount : maxValue;
        }

        return (minValue, maxValue);
    }
}