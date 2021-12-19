using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainerTests.TagPainterTests
{
    internal abstract class PainterTests
    {
        private readonly Tag[] tags = new[]
            {
                new Tag(0.3, "First"),
                new Tag(0.5, "Second"),
                new Tag(0.2, "Third")
            };
        protected ITagPainter painter;
        protected Palette palette;
        protected Func<PaintedTag, int> selector;
        protected string[] expected;

        [Test]
        public void Should_PaintCorrectly()
        {
            var result = painter.PaintTags(tags);

            result.OrderBy(selector).Select(tag => tag.Text)
                .Should().BeEquivalentTo(expected);
        }
    }
}
