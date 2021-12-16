using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class FontSizeSettings : IFontSizeSettings
    {
        public float MaxFontSize { get; set; } = 32;
        public float MinFontSize { get; set; } = 10;
    }
}