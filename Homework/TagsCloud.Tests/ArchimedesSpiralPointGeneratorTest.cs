using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization.PointGenerator;

namespace TagsCloud.Tests
{
    public class ArchimedesSpiralPointGeneratorTest
    {
        private readonly Point center = new(10, 10);
        private ArchimedesSpiralPointGenerator sut;

        [SetUp]
        public void InitGenerator()
        {
            sut = new ArchimedesSpiralPointGenerator(center);
        }

        [Test]
        public void GenerateNextPoint_OnFirstCall_ShouldReturnCenter()
        {
            var point = sut.GenerateNextPoint();

            point.Should().BeEquivalentTo(center);
        }

        [Test]
        public void GenerateNextPoint_ShouldReturnPoints_WithSameRadiuses()
        {
            var points = Enumerable
                .Range(0, 10).Select(_ => sut.GenerateNextPoint())
                .ToList();

            var radii = points.Select(x => x.GetDistance(center)).ToList();

            foreach (var (previous, current) in radii.Zip(radii.Skip(1)))
                current.Should().BeInRange(previous, previous + 1);
        }

        [Test]
        public void GenerateNextPoint_ShouldReturnPoints_WithIncreasingRadius()
        {
            var points = Enumerable.Range(0, 100)
                .Select(_ => sut.GenerateNextPoint())
                .ToList();

            var radii = points
                .Select(x => x.GetDistance(center))
                .ToList();

            foreach (var (previous, current) in radii.Zip(radii.Skip(1)))
                (current - previous).Should().BeGreaterThanOrEqualTo(0);
        }
    }
}