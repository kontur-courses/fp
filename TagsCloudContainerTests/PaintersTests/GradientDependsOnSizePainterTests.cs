using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagsCloudContainerTests
{
    public class GradientDependsOnSizePainterTests
    {
        [Test]
        public void GradientDependsOnSizePainter_ShouldReturnCorrectColors()
        {
            var painter = new GradientDependsOnSizePainter();
            var result = painter.GetWordColorDictionary(
                new Dictionary<string, int> {{"day", 3}, {"night", 1}, {"who", 2}},
                Color.Black);
            result.Should().BeEquivalentTo(new Dictionary<string, Color>
            {
                {"day", Color.FromArgb(255, 0, 0, 0)},
                {"who", Color.FromArgb(170, 0, 0, 0)},
                {"night", Color.FromArgb(85, 0, 0, 0)}
            });
        }
    }
}