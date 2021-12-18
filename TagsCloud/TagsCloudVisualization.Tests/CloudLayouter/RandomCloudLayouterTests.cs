using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Utils;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.VectorsGenerator;

namespace TagsCloudVisualization.Tests.CloudLayouter
{
    public class RandomCloudLayouterTests : LayouterTests
    {
        private Point _center;

        [SetUp]
        public void Setup()
        {
            _center = new Point(0, 0);
            _rectangles = Array.Empty<Rectangle>();
        }

        [Timeout(3_000)]
        [Test]
        public void Should_BeObtainedBySizeRange()
        {
            var random = new Random();
            var sizeRange = PositiveSize.Create(50, 50).GetValueOrThrow();
            var container = Rectangle.FromLTRB(
                _center.X - sizeRange.Width,
                _center.Y - sizeRange.Height,
                _center.X + sizeRange.Width,
                _center.Y + sizeRange.Height);

            var vectorGenerator = RandomVectorsGenerator.Create(random, sizeRange).GetValueOrThrow();
            var layouter = new NonIntersectedLayouter(_center, vectorGenerator);
            var size = new Size(5, 5);

            _rectangles = Enumerable.Range(0, 50).Select(_ => layouter.PutNextRectangle(size).GetValueOrThrow())
                .ToArray();

            _rectangles.Should().OnlyContain(rect => container.Contains(rect));
        }
    }
}