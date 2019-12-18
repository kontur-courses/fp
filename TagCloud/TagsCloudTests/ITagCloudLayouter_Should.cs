using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.CloudLayouters;
using TagsCloud.Interfaces;

namespace TagsCloudTests
{
    [TestFixture(typeof(MiddleRectangleCloudLayouter))]
    [TestFixture(typeof(CircularCloudLayouter))]
    public class ITagCloudLayouter_Should<TCloudLayouter> where TCloudLayouter: ITagCloudLayouter, new()
    {
        private ITagCloudLayouter CloudLayouter;
        private readonly Point center = new Point(0, 0);
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            CloudLayouter = new TCloudLayouter();
            rectangles = new List<Rectangle>();
        }

        [Test]
        public void ResultNotSuccess_WhenSizeHaveNegativeNumber()
        {
            var rectangleSize = new Size(-1, 100);
            CloudLayouter.PutNextRectangle(rectangleSize).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void FirstRectangle_MustBeNearCenter()
        {
            var rectangleSize = new Size(100, 100);
            var rectangle = CloudLayouter.PutNextRectangle(rectangleSize);
            rectangle.IsSuccess.Should().BeTrue();
            var deltaX = Math.Abs(rectangle.GetValueOrThrow().X - center.X);
            var deltaY = Math.Abs(rectangle.GetValueOrThrow().Y - center.Y);
            rectangles.Add(rectangle.GetValueOrThrow());
            deltaX.Should().BeLessThan(100);
            deltaY.Should().BeLessThan(100);
        }

        [TestCase(2)]
        [TestCase(20)]
        [TestCase(200)]
        public void Rectangles_Should_NotIntersectWithPrevious(int countRectangles)
        {
            var rectangleSize = new Size(100, 100);
            for (var i = 0; i < countRectangles; i++)
            {
                var rect = CloudLayouter.PutNextRectangle(rectangleSize);
                rect.IsSuccess.Should().BeTrue();
                rectangles.Any(previousRectangle => rect.GetValueOrThrow().IntersectsWith(previousRectangle)).Should().Be(false);
                rectangles.Add(rect.GetValueOrThrow());
            }
        }

        [Test]
        public void Rectangles_Should_BeInCircle()
        {
            var rectangleSize = new Size(123, 112);
            for (var i = 0; i < 100; i++)
            {
                var rect = CloudLayouter.PutNextRectangle(rectangleSize);
                rect.IsSuccess.Should().BeTrue();
                rectangles.Add(rect.GetValueOrThrow());
            }
            var maxY = rectangles.Max(rect => rect.Bottom);
            var minY = rectangles.Min(rect => rect.Top);
            var maxX = rectangles.Max(rect => rect.Right);
            var minX = rectangles.Min(rect => rect.Left);
            Math.Abs((maxY - minY) - (maxX - minX)).Should().BeLessThan(100 * (100 / 10));
        }
    }
}