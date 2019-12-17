using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Logic;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [OneTimeSetUp]
        public void InitializeLayouter()
        {
            var defaultContainer = EntryPoint.InitializeContainer();
            layouter = defaultContainer.Resolve<ILayouter>() as CircularCloudLayouter;
        }

        [TearDown]
        public void ResetLayouter()
        {
            layouter.Reset();
        }

        [TestCase(0, 1, TestName = "Width is zero")]
        [TestCase(1, 0, TestName = "Height is zero")]
        [TestCase(-1, 1, TestName = "Width is negative number")]
        [TestCase(1, -1, TestName = "Height is negative number")]
        public void PutNextRectangle_ReturnsFail(int width, int height)
        {
            var result = layouter.PutNextRectangle(new Size(width, height));
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithPositionShiftedByOffsets()
        {
            var recSize = new Size(10, 10);
            var expectedShiftedCenter = new Point(-recSize.Width / 2, -recSize.Height / 2);
            var rectangle = layouter.PutNextRectangle(recSize).GetValueOrThrow();

            new Point(rectangle.X, rectangle.Y).Should().Be(expectedShiftedCenter);
        }

        [TestCase(1, 1, TestName = "Rectangle is the smallest possible rectangle")]
        [TestCase(42, 19, TestName = "Rectangle with non-unique sizes")]
        [TestCase(int.MaxValue, int.MaxValue, TestName = "Rectangle has maximum size")]
        public void PutNextRectangle_ReturnsRectangleWithCorrectSize(int width, int height)
        {
            var rectSize = new Size(width, height);

            var rectangle = layouter.PutNextRectangle(rectSize).GetValueOrThrow();

            rectangle.Size.Should().Be(rectSize);
        }


        [TestCase(2, TestName = "2 rectangles created")]
        [TestCase(10, TestName = "10 rectangles created")]
        [TestCase(100, TestName = "100 rectangles created")]
        public void PutNextRectangles_ShouldNotReturnCrossingRectangles(int rectanglesAmount)
        {
            var rectangles = Enumerable
                .Range(1, rectanglesAmount)
                .Select(sideSize => new Size(sideSize, sideSize))
                .Select(size => layouter.PutNextRectangle(size).GetValueOrThrow())
                .ToArray();

            for (var i = 0; i < rectangles.Length; i++)
                for (var j = 0; j < i; j++)
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleCloseToCenter_IfSmallRectangleIsAfterLargeRectangle()
        {
            var centerRect = layouter.PutNextRectangle(new Size(5, 5)).GetValueOrThrow();
            layouter.PutNextRectangle(new Size(100, 100));
            var smallRect = layouter.PutNextRectangle(new Size(5, 5)).GetValueOrThrow();

            var distance = Math.Sqrt(Math.Pow(smallRect.X - centerRect.X, 2) + Math.Pow(smallRect.Y - centerRect.Y, 2));

            distance.Should().BeLessThan(10);
        }

        [TestCase(100, 1000, TestName = "100 rectangles created in less than 1 second")]
        [TestCase(500, 5000, TestName = "500 rectangles created in less than 5 second")]
        [TestCase(1000, 10000, TestName = "1000 rectangles created in less than 10 second")]
        public void PutNextRectangle_IsTimePermissible(int rectanglesAmount, int milliseconds)
        {
            var rnd = new Random();
            var timesToRepeatTest = 3;
            Action<int> action = testNumber =>
            {
                Enumerable
                    .Range(0, rectanglesAmount)
                    .Select(i => new Size(5 + rnd.Next(40), 5 + rnd.Next(40)))
                    .ForEach(size => layouter.PutNextRectangle(size));
                layouter.Reset();
            };

            var watch = new Stopwatch();
            watch.Start();
            Enumerable.Range(0, timesToRepeatTest).ForEach(action);
            watch.Stop();
            var averageTestTime = watch.ElapsedMilliseconds / timesToRepeatTest;

            averageTestTime.Should().BeLessThan(milliseconds);
        }
    }
}