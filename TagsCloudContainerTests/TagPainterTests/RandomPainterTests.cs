using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainerTests.TagPainterTests
{
    internal class RandomPainterTests
    {
        private readonly Tag[] tags = new[]
            {
                new Tag(0.2, "First"),
                new Tag(0.4, "Second"),
                new Tag(0.1, "Third"),
                new Tag(0.2, "Fourth"),
                new Tag(0.1, "Fifth")
            };
        protected ITagPainter painter;
        protected Palette palette;
        protected Func<PaintedTag, int> selector;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            palette = settings.Palette;
            painter = new RandomTagPainter();
            selector = tag => tag.Color.R;
        }

        [Test]
        public void Should_PaintCorrectly()
        {
            var result = painter.Paint(tags);

            result.All(tag => result.All(other
                => other.Color != tag.Color
                || other.Text == tag.Text)).Should().BeTrue();
        }
    }
}
