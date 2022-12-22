using System.Drawing;

namespace TagsCloudContainer.App.Layouter
{
    public class TagInfo
    {
        public string TagText { get; }
        public Font TagFont { get; }
        public Rectangle TagRect { get; }

        public TagInfo(string tagText, Font tagFont, Rectangle tagRect)
        {
            TagText = tagText;
            TagFont = tagFont;
            TagRect = tagRect;
        }
    }
}