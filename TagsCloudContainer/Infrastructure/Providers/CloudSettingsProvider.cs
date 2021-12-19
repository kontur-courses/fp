using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.TagPainters;
using TagsCloudContainer.TagsCloudLayouter.Spirals;

namespace TagsCloudContainer.Infrastructure.Providers
{
    public static class CloudSettingsProvider
    {
        public static CloudSettings GetSettings()
        {
            return new CloudSettings
            {
                Painter = painter,
                Spiral = spiral
            };
        }

        private static readonly Settings.Settings settings = SettingsProvider.GetSettings();

        private static readonly ITagPainter painter = new PrimaryTagPainter(settings);

        private static readonly ISpiral spiral = new ArchimedeanSpiral(settings);
    }
}
