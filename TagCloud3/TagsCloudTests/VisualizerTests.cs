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
        [SetUp]
        public void SetUp()
        {
            SettingsStorage.AppSettings = new();
            SettingsStorage.AppSettings.DrawingSettings = new();
        }
        [Test]
        public void Draw_WithEmptyText_ShouldReturnImage()
        {
            var size = new Size(100, 100);
            SettingsStorage.AppSettings = new();
            SettingsStorage.AppSettings.DrawingSettings = new CloudDrawingSettings
            {
                Size = size,
                FontFamily = new FontFamily("Arial"),
                FontSize = 12
            };

            var image = Painter.Draw(Enumerable.Empty<Result<TextImage>>());

            image.Should().NotBeNull();
            image.GetValueOrDefault().Width.Should().Be(size.Width);
            image.GetValueOrDefault().Height.Should().Be(size.Height);
        }
    }
}