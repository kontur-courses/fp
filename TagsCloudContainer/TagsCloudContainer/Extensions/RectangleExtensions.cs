namespace TagsCloudContainer.Extensions
{
    public static class RectangleExtensions
    {
        public static bool FitsIn(this Rectangle rect, int cellWidth, int cellHeight)
        {
            return rect.X > 0 && rect.X + rect.Width < cellWidth 
                              && rect.Y > 0 && rect.Y + rect.Height < cellHeight;
        }
    }
}
