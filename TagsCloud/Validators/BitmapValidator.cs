using System.Drawing;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class BitmapValidator : IBitmapValidator
    {
        public ResultHandler<Rectangle> ValidateNewRectangle(Rectangle rect, Bitmap bitmap)
        {
            var bitHeight = bitmap.Height;
            var bitWidth = bitmap.Width;

            var diagonalPoint = new Point(rect.X + rect.Width, rect.Y + rect.Height);

            var handler = new ResultHandler<Rectangle>(rect);

            if (rect.Height <= 0 ||
                rect.Width <= 0 ||
                rect.X < 0 ||
                rect.Y < 0)
            {
                return handler.Fail("Rectangle has negative params or zero edge");
            }

            if (rect.X > 0 && rect.X < bitWidth &&
                rect.Y > 0 && rect.Y < bitHeight &&
                diagonalPoint.X > 0 && diagonalPoint.X < bitWidth &&
                diagonalPoint.Y > 0 && diagonalPoint.Y < bitHeight)
                return handler;

            return handler.Fail("Rectangle out of bitmap bounds");
        }
    }
}