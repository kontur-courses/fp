using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagsCloudContainerTests
{
    public class DefaultWordsStainerPainter
    {
        private DefaultWordsPainter painter;

        [SetUp]
        public void SetUp()
        {
            painter = new DefaultWordsPainter();
        }

        [Test]
        public void DefaultWordStainer_ShouldReturnSequenceWithOneColor()
        {
            var result =
                painter.GetColorsSequence(new Dictionary<string, int> {{"asdx", 2}, {"ssds", 1}, {"ssdsas", 1}},
                    Color.Black).Value;
            result.Should().BeEquivalentTo(new Color[] {Color.Black, Color.Black, Color.Black});
        }
    }
}