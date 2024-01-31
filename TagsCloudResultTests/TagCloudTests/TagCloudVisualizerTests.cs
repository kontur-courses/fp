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
        var args = new ApplicationArguments { Input = "src/words1000.txt" };
        var imageGenerator = new ImageGenerator(args);
        var circularCloudLayouter = new CircularCloudLayouter(args);

        var freqDict = new WordDataSet().CreateFrequencyDict(args.Input);

        new TagCloudVisualizer(circularCloudLayouter, imageGenerator)
            .GenerateTagCloud(freqDict).IsErr.Should().BeTrue();
    }
}