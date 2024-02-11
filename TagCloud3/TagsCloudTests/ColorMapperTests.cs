using FluentAssertions;
using System.Drawing;
using TagsCloudContainer;

namespace TagsCloudTests
{
    [TestFixture]
    public class ColorMapperTests
    {
        [Test]
        public void MapColors_AssignAllColors()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var colors = new List<Color> { Color.Red, Color.Green, Color.Blue };

            var colorMapping = ColorMapper.MapColors(numbers, colors);

            colorMapping.Should().NotBeNull();
            colorMapping.Values.Distinct().Count().Should().Be(colors.Count);
            colorMapping.Values.Should().Contain(Color.Red);
            colorMapping.Values.Should().Contain(Color.Green);
            colorMapping.Values.Should().Contain(Color.Blue);
        }

        [Test]
        public void MapColors_AssignAllNumbers()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var colors = new List<Color> { Color.Red, Color.Green, Color.Blue };

            var colorMapping = ColorMapper.MapColors(numbers, colors);

            colorMapping.Should().NotBeNull();
            colorMapping.Count.Should().Be(numbers.Count);
            colorMapping.Keys.Should().Contain(1);
            colorMapping.Keys.Should().Contain(2);
            colorMapping.Keys.Should().Contain(3);
            colorMapping.Keys.Should().Contain(4);
            colorMapping.Keys.Should().Contain(5);
        }

        [Test]
        public void MapColors_WithEmptyParameters_ShouldReturnEmptyColorMapping()
        {
            var numbers = new List<int>();
            var colors = new List<Color>();

            var colorMapping = ColorMapper.MapColors(numbers, colors);

            colorMapping.Should().BeEmpty();
        }
    }
}