using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.TextHandlers;
using TagCloudApplication;
using TagCloudTests;

namespace TagCloud;

public class TagCloudGenerator
{
    private readonly ITextHandler handler;
    private readonly CircularCloudLayouter layouter;
    private readonly ICloudDrawer drawer;
    private readonly TextMeasurer measurer;

    public TagCloudGenerator(ITextHandler handler, CircularCloudLayouter layouter, ICloudDrawer drawer, TextMeasurer measurer)
    {
        this.handler = handler;
        this.layouter = layouter;
        this.drawer = drawer;
        this.measurer = measurer;
    }

    public Result<None> Generate()
    {
        var wordsResult = handler.Handle();
        if (!wordsResult.IsSuccess) 
            return Result.Fail<None>(wordsResult.Error);
        
        var rectangles = wordsResult.Value
            .OrderByDescending(pair => pair.Value)
            .Select(PutTextInRectangle)
            .ToList();
        if (rectangles.Any(result => !result.IsSuccess))
            return Result.Fail<None>("Can't place some rectangles");
        
        return drawer.Draw(rectangles.Select(result => result.Value).ToList());
    }

    private Result<TextRectangle> PutTextInRectangle(KeyValuePair<string, int> pair)
    {
        var measureResult = Result.Of(() => measurer.GetTextRectangleSize(pair.Key, pair.Value));
        if (!measureResult.IsSuccess)
            return Result.Fail<TextRectangle>(measureResult.Error);

        var (size, font) = measureResult.Value;
        return layouter.PutNextRectangle(size)
            .Then(rect => new TextRectangle(rect, pair.Key, font));
    }
}