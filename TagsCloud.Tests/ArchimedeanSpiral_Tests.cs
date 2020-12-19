using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Spirals;

namespace TagsCloud.Tests
{
    internal class ArchimedeanSpiral_Tests
    {
        private ArchimedeanSpiral spiral;
        private SpiralSettings spiralSettings;

        [SetUp]
        public void SetUp()
        {
            spiralSettings = new SpiralSettings();
            spiral = new ArchimedeanSpiral(new Point(), spiralSettings);
        }

        [Test]
        public void InitializeSpiralSettings_SetMinSpiralParameter_WhenNotPositiveSpiralParameter()
        {
            spiralSettings = new SpiralSettings {SpiralParameter = -1};

            spiralSettings.SpiralParameter.Should().Be(0.001);
        }

        [Test]
        public void GetNextPoint_FirstPointEqualsCenter()
        {
            var center = new Point(-1, 2);
            spiral = new ArchimedeanSpiral(center, spiralSettings);
            spiral.GetNextPoint().Should().Be(center);
        }

        [TestCase(10, TestName = "WhenGet10Point")]
        [TestCase(100, TestName = "WhenGet100Point")]
        [TestCase(1000, TestName = "WhenGet1000Point")]
        public void GetNextPoint_AllPointsShouldBeDifferent(int count)
        {
            var points = Enumerable.Range(0, count).Select(_ => spiral.GetNextPoint()).ToList();

            foreach (var point in points)
                points.Where(x => x != point).Any(x => x == point).Should().BeFalse();
        }
    }
}