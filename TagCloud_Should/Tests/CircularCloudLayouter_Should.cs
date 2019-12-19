using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.CloudLayouter.CircularLayouter;
using TagCloud.ErrorHandler;
using TagCloud.Visualization;

namespace TagCloud_Should.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private SpiralSettings spiralSettings;

        [SetUp]
        public void SetNewSettings()
        {
            spiralSettings = new SpiralSettings();
        }

        [Test]
        public void PutNextRectangle_FirstRect_ShouldBeInCenterWithSomeBias()
        {
            var imageSettings = new ImageSettings(new ErrorHandler())
                {ImageSize = new Size(600, 600)};
            layouter = new CircularCloudLayouter(new ArchimedeanSpiral(spiralSettings), imageSettings);
            var rectangle = layouter.PutNextRectangle(new Size(10, 3)).GetValueOrThrow();
            rectangle.Location.Should().BeEquivalentTo(new Point(7, 3));
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_NonPositiveRectSize_ResultIsNotSuccess(int width, int height)
        {
            var imageSettings = new ImageSettings(new ErrorHandler())
                {ImageSize = new Size(600, 600)};
            layouter = new CircularCloudLayouter(new ArchimedeanSpiral(spiralSettings), imageSettings);
            layouter.PutNextRectangle(new Size(width, height)).IsSuccess.Should().BeFalse();
        }

        [TestCase(101, 50, 100, 100)]
        [TestCase(100, 101, 200, 100)]
        [TestCase(101, 51, 100, 50)]
        public void PutNextRectangle_SizeBiggerThanScreen_ReturnsRightError(int width, int height, int screenWidth,
            int screenHeight)
        {
            var imageSettings = new ImageSettings(new ErrorHandler())
                {ImageSize = new Size(screenWidth, screenHeight)};
            layouter = new CircularCloudLayouter(new ArchimedeanSpiral(spiralSettings), imageSettings);
            layouter
                .PutNextRectangle(new Size(width, height)).Error.Should()
                .BeEquivalentTo("Rectangle is out of layouter");
        }

        [Test]
        public void PutNextRectangle_PositionOutOfScreen_ReturnsRightError()
        {
            var imageSettings = new ImageSettings(new ErrorHandler())
                {ImageSize = new Size(100, 100)};
            layouter = new CircularCloudLayouter(new ArchimedeanSpiral(spiralSettings), imageSettings);
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(new Size(10, 10));
            layouter.PutNextRectangle(new Size(10, 10))
                .Error
                .Should()
                .BeEquivalentTo("Rectangle is out of layouter");
        }
    }
}