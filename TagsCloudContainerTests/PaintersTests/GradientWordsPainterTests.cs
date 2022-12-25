using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagsCloudContainerTests
{
    public class GradientWordsPainterTests
    {
        private GradientWordsPainter painter;

        [SetUp]
        public void SetUp()
        {
            painter = new GradientWordsPainter();
        }
        [Test]
        public void GradientWordsPainter_ShouldReturnCorrectColors()
        {
            var result = painter.GetWordColorDictionary(
                new Dictionary<string, int> {{"night", 1}, {"who", 1}, {"day", 1}},
                Color.Black);
            result.Should().BeEquivalentTo(new Dictionary<string, Color>
            {
                {"day", Color.FromArgb(225, 0, 0, 0)},
                {"who", Color.FromArgb(235, 0, 0, 0)},
                {"night", Color.FromArgb(245, 0, 0, 0)}
            });
        }
        
        [Test]
        public void GradientWordsPainter_ShouldNotDependsOnWordCount()
        {
            var result = painter.GetWordColorDictionary(
                new Dictionary<string, int> {{"night", 10}, {"who", 5}, {"day", 1}},
                Color.Black);
            result.Should().BeEquivalentTo(new Dictionary<string, Color>
            {
                {"day", Color.FromArgb(225, 0, 0, 0)},
                {"who", Color.FromArgb(235, 0, 0, 0)},
                {"night", Color.FromArgb(245, 0, 0, 0)}
            });
        }
    }
}