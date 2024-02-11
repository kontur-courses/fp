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
        public void Draw_WithEmptyText_ShouldReturnImage()
        {
            var size = new Size(100, 100);
            var drawingSettings = new CloudDrawingSettings
            {
                Size = size,
                FontFamily = new FontFamily("Arial"),
                FontSize = 12
            };

            var image = Painter.Draw(drawingSettings.Size, Enumerable.Empty<Result<TextImage>>());

            image.Should().NotBeNull();
            image.GetValueOrDefault().Width.Should().Be(size.Width);
            image.GetValueOrDefault().Height.Should().Be(size.Height);
        }
    }
}