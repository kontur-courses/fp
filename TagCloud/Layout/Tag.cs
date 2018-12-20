using System.Drawing;

namespace TagsCloud.Layout
{
    public class Tag
    {
        public Tag(string word, Rectangle wordBox)
        {
            Word = word;
            WordBox = wordBox;
        }

        public string Word { get; }
        public Rectangle WordBox { get; }
    }
}