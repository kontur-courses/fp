using System.Drawing;

namespace TagCloud.settings
{
    public class TagSettings : ITagSettings
    {
        private readonly FontFamily family;
        private readonly float size;

        public TagSettings(FontFamily family, float size)
        {
            this.family = family;
            this.size = size;
        }

        public FontFamily GetFontFamily() => family;

        public float GetStartSize() => size;
    }
}