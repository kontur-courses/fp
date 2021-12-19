using NUnit.Framework;
using TagsCloudContainer.Infrastructure.Providers;
using TagsCloudContainer.Spirals;

namespace TagsCloudContainerTests.SpiralTests
{
    internal class ArchimedeanSpiralTests : SpiralTests
    {
        [SetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            center = settings.Center;
            spiral = new ArchimedeanSpiral(settings);
        }
    }
}
