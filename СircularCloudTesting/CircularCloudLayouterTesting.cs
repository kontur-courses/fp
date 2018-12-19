using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.InterfacesForSettings;
using TagsCloudVisualization.TagsCloud;
using TagsCloudVisualization.TagsCloud.CircularCloud;
using TagsCloudVisualization.TagsCloud.CloudConstruction;

namespace СircularCloudTesting
{
    [TestFixture]
    public class CircularCloudLayouterTesting
    {
        [TestFixture]
        private class Constructor
        {
            private IContainer container;

            [SetUp]
            public void Init()
            {
                container = new DependencyBuilder().CreateContainer().Build();
            }
            [TestCase(45, 100, TestName = "Correct center point")]
            public void Should_NotThrowArgumentException_When(int x, int y)
            {
                var center = new Point(x, y);
                var tagsCloudSettings = container.Resolve<ITagsCloudSettings>();
                tagsCloudSettings.ImageSettings.Center = center;
                tagsCloudSettings.ImageSettings.ImageSize = new Size(2000, 2000);
                var cloud = new CircularCloudLayouter(tagsCloudSettings);
                Assert.AreEqual(new Point(x, y), cloud.Center);
            }

            [Test]
            public void Should_InitializeRectangles()
            {
                var tagsCloudSettings = container.Resolve<ITagsCloudSettings>();
                tagsCloudSettings.ImageSettings.Center = new Point(0, 0);
                tagsCloudSettings.ImageSettings.ImageSize = new Size(2000, 2000);
                var cloud = new CircularCloudLayouter(tagsCloudSettings);
                Assert.AreEqual(new List<Rectangle>(), cloud.Rectangles);
            }
        }

        [TestFixture]
        private class PutNextRectangle
        {
            private CircularCloudLayouter cloud;
            private double stepAngle;
            private CircularCloudTestVisualizer cloudVisualizer;

            [SetUp]
            public void Init()
            {
                stepAngle = PointGenerator.StepAngle;
                var container = new DependencyBuilder().CreateContainer().Build();
                var tagsCloudSettings = container.Resolve<ITagsCloudSettings>();
                tagsCloudSettings.ImageSettings.Center = new Point(1000, 1000);
                tagsCloudSettings.ImageSettings.ImageSize = new Size(2000, 2000);
                tagsCloudSettings.TypeTagsCloud = TypeTagsCloud.CompressedTagsCloud;
                cloud = new CircularCloudLayouter(tagsCloudSettings);

                cloudVisualizer = new CircularCloudTestVisualizer();
            }

            [TearDown]
            public void TearDown()
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    var bmp = cloudVisualizer.DrawCircularCloud(cloud);
                    var path = $"{AppDomain.CurrentDomain.BaseDirectory}/" +
                               $"{TestContext.CurrentContext.Test.FullName}.bmp";
                    bmp.Save(path);
                    var message = $"Tag cloud visualization saved to file<{path}>";
                    Console.WriteLine(message);
                }
            }

            [Test]
            public void Should_ReturnCorrectFirstRectangle()
            {
                var rectangle = cloud.PutNextRectangle(new Size(5, 20));

                Assert.AreEqual(new Rectangle(998, 990, 5, 20), rectangle);
            }

            [Test]
            public void Should_CorrectAngleChange()
            {
                cloud.PutNextRectangle(new Size(5, 20));
                Assert.AreEqual(stepAngle, cloud.RectangleGenerator.PointGenerator.Angle);
            }

            [Test]
            public void Should_CorrectlyPositionTwoRectangles()
            {
                var sizes = new List<Size>() { new Size(10, 10), new Size(15, 15) };
                var rectangles = sizes.Select(size => cloud.PutNextRectangle(size));
                var maxDistance = rectangles.SelectMany(GetDistanceFromCenterToRectangleTops).Max();
                maxDistance.Should().BeLessThan(28.5);
            }

            [Test]
            public void Should_HaveCircleShape()
            {
                const int lengthEdge = 40;
                var sizes = new List<Size>();
                for (var i = 0; i < 30; i++)
                {
                    sizes.Add(new Size(lengthEdge, lengthEdge));
                }

                var area = lengthEdge * lengthEdge * sizes.Count;
                var radius = Math.Sqrt(area / Math.PI);
                var rectangles = sizes.Select(size => cloud.PutNextRectangle(size)).ToList();
                var maxDistance = rectangles.SelectMany(GetDistanceFromCenterToRectangleTops).Max();

                maxDistance.Should().BeLessThan(radius * 1.4);
            }

            private List<double> GetDistanceFromCenterToRectangleTops(Rectangle rect)
            {
                return new List<double>
                    {
                        cloud.Center.CalculateDistanceBetweenTwoPoints(rect.Location),
                        cloud.Center.CalculateDistanceBetweenTwoPoints(new Point(rect.X + rect.Width, rect.Y)),
                        cloud.Center.CalculateDistanceBetweenTwoPoints(new Point(rect.X, rect.Y + rect.Height)),
                        cloud.Center.CalculateDistanceBetweenTwoPoints(new Point(rect.X + rect.Width, rect.Y + rect.Height))
                    };
            }


            [Test]
            public void Should_CompressCloud()
            {
                const int lengthEdge = 40;
                var sizes = new List<Size>();
                for (var i = 0; i < 60; i++)
                {
                    sizes.Add(new Size(lengthEdge, lengthEdge));
                }

                var rectangles = sizes.Select(size => cloud.PutNextRectangle(size)).ToList();
                var hasNeighbor = rectangles.All(rect => rectangles.Any(rect1 => IsTouchingOtherRectangle(rect, rect1)));

                hasNeighbor.Should().BeTrue();
            }

            private bool IsTouchingOtherRectangle(Rectangle rect, Rectangle rect1)
            {
                return rect.X == rect1.X + rect1.Width ||
                       rect.X + rect.Width == rect1.X ||
                       rect.Y == rect1.Y + rect1.Height ||
                       rect.Y + rect.Height == rect1.Y;
            }
        }
    }
}