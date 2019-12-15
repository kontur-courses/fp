using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudGenerator.RectanglesLayouters;
using Autofac;

namespace TagsCloudGenerator_Tests.RectanglesLayouters_Tests
{
    internal class NotIntersectedRectanglesLayouter_Tests
    {
        private NotIntersectedRectanglesLayouter sut;

        private SingletonScopeInstancesContainer container;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            container = new SingletonScopeInstancesContainer();
        }

        [SetUp]
        public void SetUp()
        {
            sut = container.Get<NotIntersectedRectanglesLayouter>();
        }

        [TearDown]
        public void TearDown()
        {
            sut.Reset().IsSuccess.Should().BeTrue();
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        [TestCase(-5, -7)]
        [TestCase(0, 0)]
        public void WhenAnyRectSizeIsZeroOrNegative_ShouldThrow(int rectWidth, int rectHeight)
        {
            Action act = () => sut.PutNextRectangle(new SizeF(rectWidth, rectHeight));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 10)]
        [TestCase(0.01f, 0.01f)]
        public void ReturnedRectangleSize_ShouldBeEqualsSizeFromArgument(float rectWidth, float rectHeight)
        {
            var size = new SizeF(rectWidth, rectHeight);

            var rectResult = sut.PutNextRectangle(size);

            rectResult.IsSuccess.Should().BeTrue();
            rectResult.Value.Size.Should().Be(size);
        }

        [TestCase(5, 10)]
        [TestCase(6, 103)]
        [TestCase(8, 6)]
        [TestCase(7, 5)]
        public void CenterOfFirstRectangle_ShouldBeZeroCentralPoint(int rectWidth, int rectHeight)
        {
            var center = new PointF(0, 0);
            var rectResult = sut.PutNextRectangle(new SizeF(rectWidth, rectHeight));
            var rectCenter = new PointF(
                rectResult.Value.X + rectResult.Value.Width / 2,
                rectResult.Value.Y + rectResult.Value.Height / 2);

            rectResult.IsSuccess.Should().BeTrue();
            rectCenter.Should().Be(center);
        }

        [TestCase(100)]
        [TestCase(20)]
        [TestCase(5)]
        public void MultipleRectangles_ShouldNotIntersectEachOther(int rectanglesCount)
        {
            var rectangles = new List<RectangleF>();
            for (var i = 1; i <= rectanglesCount; i++)
            {
                var rectResult = sut.PutNextRectangle(new SizeF((i % 6 + 1) * 7, (i % 7 + 1) * 3));
                rectangles.Add(rectResult.IsSuccess ? rectResult.Value : throw new InvalidOperationException());
            }

            foreach (var rect in rectangles)
                rectangles
                    .Where(r => !r.Equals(rect))
                    .Any(r => r.IntersectsWith(rect))
                    .Should().BeFalse();
        }
    }
}