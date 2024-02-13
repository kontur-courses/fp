using System.Drawing;
using TagsCloudResult.Image;

namespace TagsCloudResult.TagCloud;

public class TagCloudVisualizer(ICircularCloudLayouter circularCloudLayouter, ImageGenerator imageGenerator)
{
    public MyResult GenerateTagCloud(IEnumerable<(string word, int count)> frequencyDict)
    {
        var wordsFrequenciesOutline = new List<(string word, int frequency, Rectangle outline)>();

        foreach (var kvp in frequencyDict)
        {
            var outer = imageGenerator.GetOuterRectangle(kvp.word, kvp.count);

            var rectangle = circularCloudLayouter.PutNextRectangle(outer);

            if (imageGenerator.RectangleOutOfResolution(rectangle))
                return MyResult.Err("Tag cloud out of image resolution");

            wordsFrequenciesOutline.Add((kvp.word, kvp.count, rectangle));
        }

        var drawTagCloudResult = imageGenerator.DrawTagCloud(wordsFrequenciesOutline);
        return drawTagCloudResult.IsErr
            ? drawTagCloudResult
            : MyResult.Ok();
    }
}