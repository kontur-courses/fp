using FluentAssertions;
using ResultOf;
using System.Drawing;
using TagsCloudContainer;
using TagsCloudContainer.Drawer;
using TagsCloudContainer.SettingsClasses;

namespace TagsCloudTests
{
    [TestFixture]
    public class VisualizerTests
    {
        [Test]
        public void Draw_WithValidParameters_ShouldReturnCorrectImage()
        {
            // Arrange
            var size = new Size(100, 100);
            var words = new List<(string, int)>
            {
                ("TestWord1", 1),
                ("TestWord2", 2)
            };
            var drawingSettings = new CloudDrawingSettings
            {
                Size = size,
                FontFamily = new FontFamily("Arial"),
                FontSize = 12
            };

            // Act
            var textImages = Visualizer.Draw(
                size,
                words.Select(x => new TextImage(x.Item1,
                                  new Font(drawingSettings.FontFamily, drawingSettings.FontSize + x.Item2),
                                    new Size(x.Item2, x.Item2), Color.White, new Point(0, 0)).AsResult()).ToList());

            // Assert
            textImages.Should().NotBeNull();
            textImages.GetValueOrThrow().Width.Should().Be(size.Width);
            textImages.GetValueOrThrow().Height.Should().Be(size.Height);
        }

        [Test]
        public void Draw_WithEmptyParameters_ShouldReturnImage()
        {
            // Arrange
            var size = new Size(100, 100);
            var drawingSettings = new CloudDrawingSettings
            {
                Size = size,
                FontFamily = new FontFamily("Arial"),
                FontSize = 12
            };

            // Act
            var image = Visualizer.Draw(size, Enumerable.Empty<Result<TextImage>>());

            // Assert
            image.Should().NotBeNull();
            image.GetValueOrThrow().Width.Should().Be(size.Width);
            image.GetValueOrThrow().Height.Should().Be(size.Height);
        }
    }
}