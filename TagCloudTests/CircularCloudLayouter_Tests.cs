using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.CloudLayouter;
using TagCloud.PointGenerator;


namespace TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CloudLayouter sut;

        [SetUp]
        public void SetUp()
        {
            sut = new CloudLayouter(new Circle(0.01f, 1, new Cache()));
        }

        [TestCase(0, 1, TestName = "Width is zero")]
        [TestCase(-1, 1, TestName = "Width is negative")]
        [TestCase(1, 0, TestName = "Height is zero")]
        [TestCase(1, -1, TestName = "Height is negative")]
        public void PutNextRectangle_ShouldReturnFailResult_IfIncorrectSize(int width, int height)
        {
            var result = sut.PutNextRectangle(new Size(width, height));

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Height and weight should be positive");
        }

        [Test]
        public void PutNextRectangle_ShouldPutWithoutIntersecting()
        {
            var cloud = new List<RectangleF>();
            for (var i = 0; i < 20; i++)
            {
                cloud.Add(sut.PutNextRectangle(new Size(50, 15)).Value);
            }

            cloud
                .Where(r => CloudIntersectWith(r, cloud))
                .Should()
                .BeEmpty();
        }

        [TestCase(2, 2, TestName = "Even width and height in zero center")]
        [TestCase(0, 0, TestName = "Odd width and height in zero center")]
        [TestCase(0, 1, TestName = "Different width and height")]
        [TestCase(3, 3, TestName = "Width and height greater than center coordinates")]
        [TestCase(3, 6, TestName = "Different parameters and different center coordinates")]
        [TestCase(-4, -3, TestName = "Negative center coordinates")]
        public void PutNextRectangle_ReturnFirstTagInCenter_IfAddOneTag(
            int width,
            int height)
        {
            sut = new CloudLayouter(new Circle(0.2f, 1,
                new Cache()));

            var tag = sut.PutNextRectangle(new Size(width, height)).Value;

            var xCenter = (tag.Left + tag.Right) / 2;
            var yCenter = (tag.Top + tag.Bottom) / 2;
            xCenter.Should().Be(0);
            yCenter.Should().Be(0);
        }

        [TestCase(1, 1, 100, 0.9, TestName = "Very tightly if small 100 1x1 squares")]
        [TestCase(2, 5, 50, 0.8, TestName = "Rectangles with different dimensions")]
        [TestCase(9, 9, 60, 0.8, TestName = "Big squares")]
        public void PutNextRectangle_ShouldPutEnoughTight(int width, int height, int count, double densityCoefficient)
        {
            var cloud = new List<RectangleF>();
            for (var i = 0; i < count; i++)
                cloud.Add(sut.PutNextRectangle(new Size(width, height)).Value);

            var density = GetDensity(sut.CloudRectangle, cloud);
            density.Should().BeGreaterThan((Math.PI / 4) * densityCoefficient).And.BeLessThan(Math.PI / 4);
        }

        private double GetDensity(RectangleF union, IEnumerable<RectangleF> cloud)
        {
            var unionRectsArea = union.Height * union.Width;
            var sumOfAreas = cloud.Sum(rectangle => rectangle.Height * rectangle.Width);
            return sumOfAreas / unionRectsArea;
        }

        private bool CloudIntersectWith(RectangleF r, IEnumerable<RectangleF> cloud)
        {
            return cloud.Where(t => t != r).Any(tag => tag.IntersectsWith(r));
        }
    }
}