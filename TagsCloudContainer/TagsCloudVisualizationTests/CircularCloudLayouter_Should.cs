using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private const float Epsilon = 1e-5f;
        private readonly Point center = new (400, 300);
        private const int RectanglesCount = 100;
        private const int CloudsCount = 100;
        private ContainerProvider provider;
        private ILayouter layouter;

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            provider = new ContainerProvider();
            provider.RegisterDependencies();
        }

        [SetUp]
        public void SetUp()
        {
            layouter = provider.Container.Resolve<ILayouter>();
        }

        [TearDown]
        public void TearDown()
        {
            var visualizer = new CircularCloudTestVisualizer(800, 600, Color.Black);
            
            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed or TestStatus.Inconclusive)
            {
                visualizer.PutRectangles(layouter.PlacedRectangles);
                var testName = TestContext.CurrentContext.Test.Name;
                Console.WriteLine($"Tags cloud visualizaton is saved to {visualizer.SaveImage(testName + ".png")}");
            }
        }

        [Test]
        public void PutRectangles_WithoutIntersections()
        {
            var rectanglesResult = layouter
                .PutNextRectangles(Enumerable.Repeat(new SizeF(10, 10), RectanglesCount))
                .Then(x => x.ToList()); // To avoid multiple enumeration

            var rectangles = rectanglesResult.GetValueOrThrow();
            
            rectangles
                .SelectMany(firstRect => rectangles
                    .Select(secondRect => firstRect != secondRect && firstRect.IntersectsWith(secondRect)))
                .Should()
                .NotContain(true);
        }

        [Test]
        public void PutNextRectangle_ThrowsArgumentException_WhenSizeIsNegative()
        {
            Action act = () => layouter.PutNextRectangle(new SizeF(-1, -1));

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_PutFirstRectangleAtCenter_WithCustomCenter()
        {
            var rectangleSize = new SizeF(50, 50);

            var rectangleResult = layouter.PutNextRectangle(rectangleSize);

            var rectangle = rectangleResult.GetValueOrThrow();

            rectangle.X.Should().BeApproximately(center.X - rectangleSize.Width / 2, Epsilon);
            rectangle.Y.Should().BeApproximately(center.Y - rectangleSize.Height / 2, Epsilon);
        }

        [Test]
        [Repeat(100)]
        public void PutNextRectangle_AverageRandomSizeRectanglesCloudDensity_GreaterThan55Percents()
        {
            var avgDensity = 0.0;

            for (var i = 0; i < CloudsCount; i++)
            {
                avgDensity += GetCloudDensity(Generators.RectanglesRandomSizeGenerator(RectanglesCount));
            }

            avgDensity /= CloudsCount;

            avgDensity.Should().BeGreaterThan(0.55);
        }

        [Test]
        [Repeat(100)]
        public void PutNextRectangle_AverageSameSizeRectanglesCloudDensity_GreaterThan65Percents()
        {
            var avgDensity = Generators.
                RectanglesRandomSizeGenerator(CloudsCount)
                .Sum(size => GetCloudDensity(Enumerable.Repeat(size, RectanglesCount)));

            avgDensity /= CloudsCount;

            avgDensity.Should().BeGreaterThan(0.65);
        }


        private double GetCloudDensity(IEnumerable<SizeF> rectanglesSizes)
        {
            layouter = provider.Container.Resolve<ILayouter>();
            
            var rectanglesArea = 0.0f;

            var lastRectangle = new RectangleF();

            foreach (var rectangleSize in rectanglesSizes)
            {
                lastRectangle = layouter.PutNextRectangle(rectangleSize).GetValueOrThrow();

                rectanglesArea += lastRectangle.GetArea();
            }

            var floatCenter = new PointF(center.X, center.Y);

            var circleRadius = floatCenter.DistanceTo(lastRectangle.Location);
            var circleArea = circleRadius * circleRadius * Math.PI;

            var density = rectanglesArea / circleArea;

            return density;
        }
    }
}