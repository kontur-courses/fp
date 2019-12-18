using System.Drawing;
using TagsCloud.FontGenerators;

namespace TagsCloud.TagGenerators
{
    public class Tag
    {
        public readonly FontSettings fontSettings;
        public readonly Color colorTag;
        public readonly string word;

        public Tag(FontSettings fontSettings, Color color, string word)
        {
            this.fontSettings = fontSettings;
            colorTag = color;
            this.word = word;
        }
    }
}
