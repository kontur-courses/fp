using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.AppSettings;
using TagsCloudVisualization.Canvases;
using TagsCloudVisualization.PointsGenerators;
using TagsCloudVisualization.TagCloudLayouter;

namespace TagsCloudVisualizationTests.TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [SetUp]
        public void SetUp()
        {
            var canvas = new Canvas(new ImageSettings {Width = 100, Height = 100});
            pointGenerator = new ArchimedesSpiral(new SpiralParams(), canvas.GetImageCenter());
            sut = new CircularCloudLayouter(pointGenerator);
            addedRectangles = new List<Rectangle>();
        }

        private ICloudLayouter sut;
        private IPointGenerator pointGenerator;
        private List<Rectangle> addedRectangles;

        [Test]
        public void GetAddedRectangle_RectCentralPointInCenter_WhenPutOneRectangle()
        {
            var size = new Size(200, 200);
            addedRectangles.Add(sut.PutNextRectangle(size).GetValueOrThrow());

            var firstAddedRect = addedRectangles.First();

            firstAddedRect.Location.Should().Be(new Point(pointGenerator.Center.X - firstAddedRect.Width / 2,
                pointGenerator.Center.Y - firstAddedRect.Height / 2));
        }

        [TestCase(0)]
        [TestCase(5)]
        public void PutNextRectangle_CorrectRectanglesCount_ReturnPutRectangles(int expectedCount)
        {
            for (int i = 0; i < expectedCount; i++)
                addedRectangles.Add(sut.PutNextRectangle(new Size(200, 200)).GetValueOrThrow());

            addedRectangles.Count().Should().Be(expectedCount);
        }

        [Test]
        public void GetAddedRectangle_ReturnAddedRectangle_WhenPutOneRectangle()
        {
            var size = new Size(100, 100);
            var expectedRects = new List<Rectangle>
            {
                new Rectangle(
                    new Point(0, 0), size)
            };
            
            addedRectangles.Add(sut.PutNextRectangle(size).GetValueOrThrow());
            
            addedRectangles.Should().Equal(expectedRects);
        }

        [TestCase(100, -100)]
        [TestCase(-100, 100)]
        [TestCase(100, 0)]
        [TestCase(0, 100)]
        public void PutNextRectangle_ReturnsFailedResult_IncorrectArguments(int width, int height)
        {
            var result = sut.PutNextRectangle(new Size(width, height));

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("Rectangle size must be more than zero");
        }

        [Test]
        public void PutNextRectangle_ReturnsFailedResult_WhenSpiralGeneratorNull()
        {
            var layouter = new CircularCloudLayouter(null);
            var result = layouter.PutNextRectangle(new Size(2, 2));

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("Point generator cannot be null");
        }
        
        [Test]
        public void PutNextRectangle_ReturnsCorrectSecondRectPosition_WhenPutTwoRectangles()
        {
            addedRectangles.Add(sut.PutNextRectangle(new Size(50, 50)).GetValueOrThrow());
            addedRectangles.Add(sut.PutNextRectangle(new Size(50, 50)).GetValueOrThrow());
            var expectedPosition = new Point(75, 19);

            var secondAddedRectangle = addedRectangles.Skip(1).First();

            secondAddedRectangle.Location.Should().Be(expectedPosition);
        }

        [Test]
        public void PutNextRectangle_PointsGenerationStartsOver_WhenPutOneRectangle()
        {
            sut.PutNextRectangle(new Size(300, 300));
        
            pointGenerator.GetNextPoint().Should().Be(pointGenerator.Center);
        }
        
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public void GetAddedRectangles_RectanglesShouldNotIntersect_WhenPutManyRectangles(int addedRectanglesCount)
        {
            for (int i = 0; i < addedRectanglesCount; i++)
                addedRectangles.Add(sut.PutNextRectangle(new Size(50, 50)).GetValueOrThrow());

            foreach (var addedRectangle in addedRectangles)
                addedRectangles.All(
                    rectangle => rectangle.IntersectsWith(addedRectangle)).Should().BeFalse();
        }
    }
}