using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloudTests
{
    internal class CircularCloudTests
    {
        private CloudLayouter layouter;
        private readonly Size size = new Size(20, 20);
        private readonly List<IPlacementStrategy> strategies = new List<IPlacementStrategy>
            {new SpiralStrategy(), new CenterMoveStrategy()};

        [SetUp]
        public void SetUp()
        {
            var point = new Point(0, 0);
            layouter = new CloudLayouter(strategies);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;
            var imagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "Tests",
                TestContext.CurrentContext.Test.Name + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-fff}" + ".bmp");
            var visualizer = new CloudVisualizer(imagePath);
            foreach (var rectangle in layouter.Rectangles)
            {
                visualizer.AddWord(new Word("test", 0), rectangle, new Font(FontFamily.GenericSerif, 20));
            }

            visualizer.CreateImage(Color.Blue, Color.White);
            TestContext.Out.WriteLine("Tag cloud visualization saved to file " + imagePath);
        }

        [Test]
        public void Constructor_ShouldCreateLayouterWithCenter()
        {
            var layouter = new CloudLayouter(strategies);
            layouter.Center.Should().Be(new Point(0, 0));
        }

        [TestCase(0, 1, TestName = "has zero width")]
        [TestCase(1, 0, TestName = "has zero height")]
        [TestCase(-1, 1, TestName = "has negative width")]
        [TestCase(1, -1, TestName = "has negative height")]
        public void PutNextRectangle_ShouldThrowExceptionWhenSize(int width, int height)
        {
            Action putRectangle = () => layouter.PutNextRectangle(new Size(width, height));

            putRectangle.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnCorrectRectangle()
        {
            var returnedValue = layouter.PutNextRectangle(size);
            returnedValue.Should().Be(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_ShouldPlaceRectangleIntoCollection()
        {
            layouter.PutNextRectangle(size);
            layouter.Rectangles.Should().Contain(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_ShouldNotReturnIntersectingRectangles()
        {
            PopulateWithRandomRectangles(layouter);

            var rectanglesChecked = 1;
            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles.Skip(rectanglesChecked++))
                {
                    RectanglesIntersect(rectangle, otherRectangle).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangle_ShouldDistributeRectanglesDensely()
        {
            const double expectedDensity = 0.6;

            PopulateWithRandomRectangles(layouter);
            var rectangles = layouter.Rectangles;
            var rectanglesArea = rectangles.Sum(r => r.Width * r.Height);
            var width = rectangles.Max(r => r.Right) - rectangles.Min(r => r.Left);
            var height = rectangles.Max(r => r.Bottom) - rectangles.Min(r => r.Top);
            var radius = Math.Max(width, height) / 2;
            var circleArea = Math.PI * Math.Pow(radius, 2);
            var density = rectanglesArea / circleArea;

            density.Should().BeGreaterOrEqualTo(expectedDensity);
        }

        [Test]
        public void PutNextRectangle_ShouldMakeCircularCloud()
        {
            PopulateWithRandomRectangles(layouter);
            var rectangles = layouter.Rectangles;
            var width = rectangles.Max(r => r.Right) - rectangles.Min(r => r.Left);
            var height = rectangles.Max(r => r.Bottom) - rectangles.Min(r => r.Top);
            var radius = Math.Max(width, height) / 2;
            var circleArea = Math.PI * Math.Pow(radius, 2);
            var rectangleArea = width * height;

            circleArea.Should().BeLessThan(rectangleArea);
        }

        private static void PopulateWithRandomRectangles(ICloudLayouter layouter)
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(
                    random.Next(50, 201),
                    random.Next(20, 61)));
            }
        }

        private static bool RectanglesIntersect(Rectangle r1, Rectangle r2)
        {
            return r1.X < r2.X + r2.Width &&
                   r2.X < r1.X + r1.Width &&
                   r1.Y < r2.Y + r2.Height &&
                   r2.Y < r1.Y + r1.Height;
        }
    }
}
