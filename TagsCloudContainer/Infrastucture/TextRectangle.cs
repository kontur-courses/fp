namespace TagsCloudContainer.Infrastucture
{
    public class TextRectangle
    {
        public RectangleF Rectangle { get; }

        public string Text { get; }

        public Font Font { get; }

        public TextRectangle(RectangleF rectangle, string text, Font font)
        {
            Rectangle = rectangle;
            Text = text;
            Font = font;
        }

        public float Area => Rectangle.Width * Rectangle.Height;

        public bool FitsIntoImage(int imageWidth, int imageHeight)
        {
            return Rectangle.X > 0 && Rectangle.Width + Rectangle.X <= imageWidth
                && Rectangle.Y > 0 && Rectangle.Height + Rectangle.Y <= imageHeight;
        }

    }
}
