using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer;

namespace TagsCloudContainerTests
{
    public class RectangleCoordinatesCalculatorTests
    {
        [Test]
        public void CalculateRectangleCoordinates_ShouldReturnCorrectCoordinates_OnCorrectInput()
        {
            var size = new Size(100, 100);
            var center = new Point(-100, -100);
            var coordinates = RectangleCoordinatesCalculator.CalculateRectangleCoordinates(center, size).Value;
            coordinates.Should().BeEquivalentTo(new Point(-150, -150));
        }

        [Test]
        public void CalculateRectangleCoordinates_ShouldReturnCorrectCoordinates_OnZeroSize()
        {
            var size = new Size(0, 0);
            var center = new Point(0, 0);
            var coordinates = RectangleCoordinatesCalculator.CalculateRectangleCoordinates(center, size).Value;
            coordinates.Should().BeEquivalentTo(new Point(0, 0));
        }

        [Test]
        public void CalculateRectangleCoordinates_ShouldThrowArgumentException_WhenSizeLessThanZero()
        {
            var size = new Size(-1, -1);
            var center = new Point(0, 0);
            var coordinates = RectangleCoordinatesCalculator.CalculateRectangleCoordinates(center, size);
            coordinates.Error.Should().Be("Incorrect size of rectangle");
        }
    }
}