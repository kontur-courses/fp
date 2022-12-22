using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Layouter;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class SpiralTest
    {
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, -5)]
        public void Creation_ZeroStep_ThrowArgumentException(int x, int y, int step)
        {
            var spiral = new Spiral(new Point(x, y));
            var result = spiral.NextPoint(step);
            result.Error.Should().Be("Step must not be less than or equal to 0");
        }

        [TestCase(0, 0, 1)]
        [TestCase(5, 5, 10)]
        [TestCase(100, 100, 50)]
        public void Creation_CorrectParameters_ShouldNotFail(int x, int y, int step)
        {
            var spiral = new Spiral(new Point(x, y));
            var result = spiral.NextPoint(step);
            result.Error.Should().Be(null);
        }

        [Test]
        public void NextPoint_ShouldGetDifferentPoint()
        {
            var spiral = new Spiral(new Point(0, 0));
            var point = spiral.NextPoint(10);
            spiral.NextPoint(10).Value.Should().NotBe(point.Value);
        }
    }
}