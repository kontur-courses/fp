using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Readers;
using TagCloud.Visualizers;

namespace TagCloudTests
{
    public class TagColoringFactoryTests
    {
        private ITagColoringFactory tagColoringFactory;
        private IEnumerable<Color> colors;

        [SetUp]
        public void SetUp()
        {
            colors = new[] {Color.Aqua};
            tagColoringFactory = new TagColoringFactory();
        }

        [Test]
        public void Create_ShouldReturnsFailResult_WhenExtensionNotSupported()
        {
            tagColoringFactory.Create("abc", colors).Error.Should().NotBeNullOrEmpty();
        }

        [TestCase("alt", typeof(AlternatingTagColoring))]
        [TestCase("random", typeof(RandomTagColoring))]
        public void Create_ShouldReturnReader(string extension, Type expectedType)
        {
            var lines = tagColoringFactory.Create(extension, colors);

            lines.GetValueOrThrow().Should().BeOfType(expectedType);
        }
    }
}
