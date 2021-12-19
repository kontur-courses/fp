using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.TagsCloudLayouter.Spirals;

namespace TagsCloudContainerTests.SpiralTests
{
    internal class OvalSpiralTests : SpiralTests
    {
        [SetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            center = settings.Center;
            spiral = new OvalSpiral(settings);
        }
    }
}
