using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.TagPainters;

namespace TagsCloudContainerTests.TagPainterTests
{
    internal class FrequencyPainterTests : PainterTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            painter = new FrequencyTagPainter(settings);
            selector = tag => tag.Color.A;
            expected = new[] { "Second", "First", "Third" };
        }
    }
}
