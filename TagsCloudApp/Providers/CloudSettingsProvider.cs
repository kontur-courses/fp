using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Spirals;
using TagsCloudContainer.TagPainters;

namespace TagsCloudApp.Providers
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
