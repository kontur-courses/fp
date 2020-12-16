using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Curves;
using TagCloud.Visualization;
using TagCloud.WordsFilter;
using TagCloud.WordsProvider;

namespace TagCloudTest
{
    [TestFixture]
    public class TagCloudVisualizerShould
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tagCloud = new CircularCloudLayouter(
                new ArchimedeanSpiral(tagCloudCenter),
                new CircularWordsProvider(),
                new WordsFilter());
            tagCloud.GenerateTagCloud();
        }

        [SetUp]
        public void SetUp()
        {
            visualizer = new TagCloudVisualizer(tagCloud);
        }

        private IVisualizer visualizer;
        private ITagCloud tagCloud;
        private const string font = "Times New Roman";
        private readonly Color[] colors = {Color.Aqua};
        private readonly Point tagCloudCenter = new Point(1920 / 2, 1080 / 2);

        [Test]
        public void ReturnError_WhenTagCloudDoesNotFitScreen()
        {
            visualizer.CreateBitMap(100, 100, colors, font).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReturnError_WhenFontIsIncorrect()
        {
            visualizer.CreateBitMap(5000, 5000, colors, "asdas").IsSuccess.Should().BeFalse();
        }
    }
}