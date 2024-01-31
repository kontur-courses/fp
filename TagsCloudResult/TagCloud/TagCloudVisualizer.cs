using System.Drawing;
using ResultSharp;
using TagsCloudResult.Image;

namespace TagsCloudResult.TagCloud;

public class TagCloudVisualizer(ICircularCloudLayouter circularCloudLayouter, ImageGenerator imageGenerator)
{
    public Result GenerateTagCloud(IEnumerable<(string word, int count)> frequencyDict)
    {
        var wordsFrequenciesOutline = new List<(string word, int frequency, Rectangle outline)>();

        var rectangleOutOfResult = Result.Ok();
        
        foreach (var kvp in frequencyDict)
        {
            var outerResult = imageGenerator.GetOuterRectangle(kvp.word, kvp.count);
            if (outerResult.IsErr) return Result.Err(outerResult.UnwrapErr());
            
            var rectangle = circularCloudLayouter.PutNextRectangle(outerResult.Unwrap());

            if (rectangleOutOfResult.IsOk) rectangleOutOfResult = imageGenerator.RectangleOutOfResolution(rectangle);
            
            wordsFrequenciesOutline.Add((kvp.word, kvp.count, rectangle));
        }

        imageGenerator.DrawTagCloud(wordsFrequenciesOutline);
        
        return rectangleOutOfResult.IsOk ? Result.Ok() : Result.Err(rectangleOutOfResult.UnwrapErr());
    }
}