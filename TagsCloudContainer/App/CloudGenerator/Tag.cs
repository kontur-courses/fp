using System.Drawing;

namespace TagsCloudContainer.App.CloudGenerator
{
    internal class Tag
    {
        public readonly double FontSize;
        public readonly Rectangle Rectangle;
        public readonly string Word;

        public Tag(string word, double fontSize, Rectangle rectangle)
        {
            Word = word;
            FontSize = fontSize;
            Rectangle = rectangle;
        }
    }
}