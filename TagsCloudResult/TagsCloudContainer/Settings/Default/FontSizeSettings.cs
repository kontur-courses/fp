namespace TagsCloudContainer.Settings.Default
{
    public class FontSizeSettings : IFontSizeSettings
    {
        public float MaxFontSize { get; set; } = 32;
        public float MinFontSize { get; set; } = 10;
    }
}