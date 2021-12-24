using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudContainer.Tests
{
    public class SpiralPointsGeneratorTests
    {
        [Test]
        public void Constructor_Fails_WhenCenterCoordinatesAreNegative()
        {
            new SpiralPointsGenerator(new Point(-1, -1), 0, 0, 1, 1).GetSpiralPoints()
                .Error.Should()
                .Be("Center coordinates can't be negative");
        }

        [Test]
        public void Constructor_Fails_WhenStartRadiusIsNegative()
        {
            new SpiralPointsGenerator(new Point(1, 1), -1, 0, 1, 1).GetSpiralPoints()
                .Error.Should()
                .Be("Radius can't be negative");
        }

        [Test]
        public void Constructor_Fails_WhenAngleDeltaIsZero()
        {
            new SpiralPointsGenerator(new Point(1, 1), 1, 0, 0, 1).GetSpiralPoints()
                .Error.Should()
                .Be("Delta can't be zero");
        }

        [Test]
        public void Constructor_Fails_WhenRadiusDeltaIsZero()
        {
            new SpiralPointsGenerator(new Point(1, 1), 1, 0, 1, 0).GetSpiralPoints()
                .Error.Should()
                .Be("Delta can't be zero");
        }

        [Test]
        public void Constructor_NotThrows_WhenParamsAreValid()
        {
            new SpiralPointsGenerator(new Point(1, 1), 1, 0, 1, 1).GetSpiralPoints().IsSuccess.Should().Be(true);
        }

        [Test]
        public void GetSpiralPoints_ReturnsCenter_WhenItIsCalledFirstTime()
        {
            var center = Point.Empty;
            var generator = new SpiralPointsGenerator(center);

            generator.GetSpiralPoints().GetValueOrThrow().FirstOrDefault().Should().Be(center);
        }

        [Test]
        public void GetSpiralPoints_ReturnsDifferentPoints()
        {
            var points = new List<Point>();
            using var enumerator = new SpiralPointsGenerator(new Point(100, 100), 10, 0, 0.1, 1).GetSpiralPoints()
                .GetValueOrThrow()
                .GetEnumerator();

            enumerator.MoveNext();
            for (var i = 0; i < 100; i++)
            {
                enumerator.MoveNext();
                points.Should().NotContain(enumerator.Current);
                points.Add(enumerator.Current);
            }
        }

        [Test]
        public void GetSpiralPoints_ReturnsPoints_WithIncreasingDistanceToCenter()
        {
            var distances = new List<double>();
            var center = new Point(100, 100);
            using var enumerator = new SpiralPointsGenerator(center, 10, 0, 0.1, 1).GetSpiralPoints()
                .GetValueOrThrow()
                .GetEnumerator();

            enumerator.MoveNext();
            for (var i = 0; i < 100; i++)
            {
                enumerator.MoveNext();
                var point = enumerator.Current;
                var distance = Math.Sqrt((center.X - point.X) * (center.X - point.X) +
                                         (center.Y - point.Y) * (center.Y - point.Y));
                distances.Should().NotContain(distance);
                distances.Add(distance);
            }
        }
    }
}