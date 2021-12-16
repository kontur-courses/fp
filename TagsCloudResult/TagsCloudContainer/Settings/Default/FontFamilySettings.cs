using System.Drawing;

namespace TagsCloudContainer.Settings.Default
{
    public class FontFamilySettings : IFontFamilySettings
    {
        public FontFamily FontFamily { get; set; } = FontFamily.GenericMonospace;
    }
}