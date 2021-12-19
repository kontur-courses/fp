using System.Collections.Generic;
using System.Drawing;
using TagCloud.Visualizers;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Creators;

namespace TagCloudTests
{
    public class CloudVisualizatorTests
    {
        private IVisualizer visualizer;
        private DrawingSettings drawingSettings;

        [SetUp]
        public void SetUp()
        {
            //var settings = new DrawingSettings(new[] { Color.Blue },
            //    Color.CornflowerBlue,
            //    new Font(FontFamily.GenericSansSerif, 8),
            //    200,
            //    200,
            //    "alt");
            drawingSettings = new DrawingSettings(new[] { Color.Blue },
                Color.CornflowerBlue,
                new Font(FontFamily.GenericSansSerif, 8),
                200,
                200,
                "alt");
            visualizer = new CloudVisualizer(drawingSettings);
        }

        [TearDown]
        public void TearDown()
        {
            drawingSettings.Dispose();
        }

        [TestCase(0, 1, TestName = "When Zero Width")]
        [TestCase(1, 0, TestName = "When Zero Height")]
        [TestCase(0, 0, TestName = "When Zero Width And Height")]
        [TestCase(-1, 1, TestName = "When Negative Width")]
        [TestCase(1, -1, TestName = "When Negative Height")]
        [TestCase(-1, -1, TestName = "When Negative Width And Height")]
        public void DrawCloud_ShouldReturns_FailResult(int width, int height)
        {
            var settings = new DrawingSettings(new[] {Color.Blue},
                Color.CornflowerBlue,
                new Font(FontFamily.GenericSansSerif, 8),
                width, height, "alt");
            var visualizer = new CloudVisualizer(settings);

            visualizer.DrawCloud(null, null).IsSuccess.Should().BeFalse();
        }

        [TestCaseSource(nameof(CasesForDrawRectangle))]
        public void DrawRectangle_Should(int pixelX, int pixelY, int expectedARGBColor)
        {
            var tag = new Tag("bbbbbbb", 1, new Size(50, 50));
            var tags = new[] { new Tag(tag, new Rectangle(new Point(50, 50), new Size()))};

            var bitmap = visualizer.DrawCloud(tags, new TagColoringFactory());

            bitmap.GetValueOrThrow().GetPixel(pixelX, pixelY).ToArgb()
                .Should().Be(expectedARGBColor);
        }

        private static IEnumerable<TestCaseData> CasesForDrawRectangle
        {
            get
            {
                yield return new TestCaseData(60, 60, Color.Blue.ToArgb())
                    .SetName("Write words");
                yield return new TestCaseData(50, 50, Color.CornflowerBlue.ToArgb())
                    .SetName("Draw background");
            }
        }
    }
}