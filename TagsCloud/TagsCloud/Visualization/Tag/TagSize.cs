using System.Drawing;

namespace TagsCloud.Visualization.Tag
{
    public class TagSize
    {
        public readonly int FontSize;
        public readonly Size RectangleSize;

        public TagSize(Size rectangleSize, int fontSize)
        {
            RectangleSize = rectangleSize;
            FontSize = fontSize;
        }
    }
}