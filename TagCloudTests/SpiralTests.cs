using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.PointGenerator;

namespace TagCloudTests;

[TestFixture]
public class SpiralTests
{
    [Test]
    public void GetCoordinates_FirstPointInCenter()
    {
        var spiral = Spiral.GetDefaultSpiral();

        var firstPoint = spiral.GetPoints(new Size(1, 1)).First();

        firstPoint.Should().Be(new PointF(0, 0));
    }

    [Test]
    public void GetCoordinates_ReturnCoordinatesThatRadiusShouldIncrease()
    {
        var spiral = Spiral.GetDefaultSpiral();

        var points = spiral.GetPoints(new Size(5, 5)).Take(100).ToArray();

        for (var i = 1; i < points.Length; i++)
        {
            var previousRadius = GetDistance(new PointF(0, 0), points[i - 1]);
            var currentRadius = GetDistance(new PointF(0, 0), points[i]);
            (previousRadius < currentRadius).Should().BeTrue();
        }
    }

    private static float GetDistance(PointF from, PointF to)
    {
        var dx = to.X - from.X;
        var dy = to.Y - from.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}