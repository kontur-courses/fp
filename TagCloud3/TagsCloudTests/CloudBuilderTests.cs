using FluentAssertions;
using System.Drawing;
using TagsCloudContainer.SettingsClasses;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    internal class CloudBuilderTests
    {
        private List<(string, int)> words;

        [SetUp]
        public void Setup()
        {
            SettingsStorage.AppSettings = new();
            SettingsStorage.AppSettings.DrawingSettings = new CloudDrawingSettings
            {
                Size = new(1000, 1000),
                FontFamily = new("Arial"),
                FontSize = 12
            };

            words = new() { ("TestWord1", 1), ("TestWord2", 2), ("TestWord3", 3) };
        }

        [Test]
        public void Initialize_WithValidSettings_ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new TagsCloudLayouter(words));
        }

        [Test]
        public void Initialize_WithInvalidSize_ShouldThrowArgumentException()
        {
            SettingsStorage.AppSettings.DrawingSettings.Size = new Size(-1, 1000);

            Assert.Throws<ArgumentException>(() => new TagsCloudLayouter(words));
        }

        [Test]
        public void GetTextImages_ShouldContainCorrectNumberOfRectangles()
        {
            var expectedCount = words.Count;

            var actualCount = new TagsCloudLayouter(words).GetTextImages().Count();

            actualCount.Should().Be(expectedCount);
        }

        [Test]
        public void GetTextImages_ShouldReturnCorrectTextImages()
        {
            var layouter = new TagsCloudLayouter(words);

            var textImages = layouter.GetTextImages().ToList();

            textImages.Should().ContainSingle(x => x.GetValueOrDefault().Text == "TestWord1");
            textImages.Should().ContainSingle(x => x.GetValueOrDefault().Text == "TestWord2");
            textImages.Should().ContainSingle(x => x.GetValueOrDefault().Text == "TestWord3");
        }

        [Test]
        public void GetTextImages_ShouldNotContainOverlappingTextImages()
        {
            var layouter = new TagsCloudLayouter(words);

            var textImages = layouter.GetTextImages().Select(x => new Rectangle(x.GetValueOrDefault().Position, x.GetValueOrDefault().Size));
            var rectangles = textImages
                 .SelectMany((x, i) => textImages.Skip(i + 1), Tuple.Create)
                 .Where(x => x.Item1.IntersectsWith(x.Item2));

            rectangles.Should().BeEmpty();
        }
    }
}
