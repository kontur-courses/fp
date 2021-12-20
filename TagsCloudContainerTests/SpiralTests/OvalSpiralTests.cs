using NUnit.Framework;
using TagsCloudApp.Providers;
using TagsCloudContainer.Spirals;

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
