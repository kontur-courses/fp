using System;
using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainerTests.TagPainterTests
{
    internal class GradientPainterTests : PainterTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            palette = settings.Palette;
            painter = new GradientTagPainter(settings);
            selector = tag => Math.Abs(tag.Color.R - palette.Primary.R);
            expected = new[] { "Third", "First", "Second" };
        }
    }
}
