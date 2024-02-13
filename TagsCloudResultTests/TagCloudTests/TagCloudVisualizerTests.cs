using FluentAssertions;
using TagsCloudResult.Image;
using TagsCloudResult.TagCloud;
using TagsCloudResult.UI;
using TagsCloudResult.Utility;

namespace TagsCloudResultTests.TagCloudTests;

[TestFixture]
public class TagCloudVisualizerTests
{
    [Test]
    public void GenerateTagCloud_Should_ReturnFail_OnBigData()
    {
        var args = new ApplicationArguments
        {
            Input = "/Users/draginsky/Rider/fp/TagsCloudResult/src/words1000.txt",
            FontPath = "/Users/draginsky/Rider/fp/TagsCloudResult/src/JosefinSans-Regular.ttf"
        };
        var imageGenerator = new ImageGenerator(args);
        var circularCloudLayouter = new CircularCloudLayouter(args);

        var freqDict = new WordDataSet()
            .CreateFrequencyDict(new FileTextHandler().ReadText(args.Input).Unwrap());

        new TagCloudVisualizer(circularCloudLayouter, imageGenerator)
            .GenerateTagCloud(freqDict).IsErr.Should().BeTrue();
    }
}