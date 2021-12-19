using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.Spirals;

namespace TagsCloudContainerTests.SpiralTests
{
    internal abstract class SpiralTests
    {
        protected Point center;
        protected ISpiral spiral;

        [Test]
        public void Should_StartAtCenter()
        {
            center.Should().BeEquivalentTo(spiral.GetNext());
        }

        [Test]
        public void Should_ResetCorrectly()
        {
            for (var i = 0; i < 100; i++)
                spiral.GetNext();
            spiral.Reset();
            center.Should().BeEquivalentTo(spiral.GetNext());
        }

        [Test]
        public void Should_IncreaseDistanceFromCenter()
        {
            var spiralPoint = spiral.GetNext();
            var distance = center.GetDistance(spiralPoint);

            for (var i = 1; i <= 1000; i++)
            {
                spiralPoint = spiral.GetNext();
                if (i % 250 == 0)
                {
                    var newDistance = center.GetDistance(spiralPoint);
                    newDistance.Should().BeGreaterThan(distance);
                    distance = newDistance;
                }
            }
        }
    }
}
