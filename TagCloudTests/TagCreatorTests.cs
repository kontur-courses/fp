using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Creators;
using TagCloud.Settings;

namespace TagCloudTests
{
    public class TagCreatorTests
    {
        private ITagCreator tagCreator;
        private ITagCreatorSettings settings;

        [SetUp]
        public void SetUp()
        {
            settings = new ProcessorSettings(new[] { "" }, "",
                "Microsoft Sans Serif",
                8,
                1, 1, "", "", "", "", "");
            tagCreator = new TagCreator(settings);
        }

        [Test]
        public void Create_ShouldCreateTags()
        {
            var words = new Dictionary<string, int>{{"a", 5}, {"b", 3}, {"c", 1}};

            var tags = tagCreator.Create(words).GetValueOrThrow().ToArray();

            var expectedSize = new Size(50, 70);
            var tag = tags.First(t => t.Size == expectedSize);
            tag.Size.Should().Be(expectedSize);
            tag.Frequency.Should().Be(5);
        }

        [Test]
        public void Create_ShouldReturnsNoTags_WhenNoWords()
        {
            var words = new Dictionary<string, int>();

            var tags = tagCreator.Create(words);

            tags.GetValueOrThrow().Count().Should().Be(0);
        }
    }
}
