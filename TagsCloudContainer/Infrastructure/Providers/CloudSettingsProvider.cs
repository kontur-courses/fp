using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.TagPainters;
using TagsCloudContainer.TagsCloudLayouter.Spirals;

namespace TagsCloudContainer.Infrastructure.Providers
{
    public static class CloudSettingsProvider
    {
        static CloudSettingsProvider()
        {
            var settings = SettingsProvider.GetSettings();
            painter = new PrimaryTagPainter(settings);
            spiral = new ArchimedeanSpiral(settings);
        }

        public static CloudSettings GetSettings()
        {
            return new CloudSettings
            {
                Painter = painter,
                Spiral = spiral
            };
        }

        private static readonly ITagPainter painter;

        private static readonly ISpiral spiral;
    }
}
