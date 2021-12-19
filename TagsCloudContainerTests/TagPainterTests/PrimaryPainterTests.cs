using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainerTests.TagPainterTests
{
    internal class PrimaryPainterTests : PainterTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            painter = new PrimaryTagPainter(settings);
            selector = tag => tag.Color.A;
            expected = new[] { "First", "Second", "Third" };
        }
    }
}
