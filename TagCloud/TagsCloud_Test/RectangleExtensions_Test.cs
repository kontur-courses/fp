using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Extensions;

namespace TagsCloud_Test
{
    public class RectangleExtensionsTest
    {
        [Test]
        public void GetDistancesToInnerPoint_ShouldBeCorrect()
        {
            var rectangle = new Rectangle(Point.Empty, new Size(100, 100));
            var point = new Point(50, 50);
            var expected = new List<int> {50, 50, 50, 50};
            rectangle.GetDistancesToInnerPoint(point).Value.Should().Equal(expected);
        }

        [Test]
        public void GetDistancesToInnerPoint_ShouldFailure_WhenPointIsOutside()
        {
            var rectangle = new Rectangle(Point.Empty, new Size(100, 100));
            var point = new Point(-1, -1);
            rectangle.GetDistancesToInnerPoint(point).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetDistancesToInnerPoint_Distances_ShouldBePositive()
        {
            var rectangle = new Rectangle(Point.Empty, new Size(100, 100));
            var point = new Point(50, 50);
            rectangle.GetDistancesToInnerPoint(point).Value.All(d => d >= 0).Should().BeTrue();
        }
    }
}