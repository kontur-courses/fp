using System.Drawing;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class BitmapValidator : IBitmapValidator
    {
        public Result<Rectangle> ValidateNewRectangle(Rectangle rect, Bitmap bitmap)
        {
            var bitHeight = bitmap.Height;
            var bitWidth = bitmap.Width;

            var diagonalPoint = new Point(rect.X + rect.Width, rect.Y + rect.Height);

            if (rect.X > 0 && rect.X < bitWidth &&
                rect.Y > 0 && rect.Y < bitHeight &&
                diagonalPoint.X > 0 && diagonalPoint.X < bitWidth &&
                diagonalPoint.Y > 0 && diagonalPoint.Y < bitHeight)
                return Result.Ok(rect);

            return Result.Fail<Rectangle>("Rectangle out of bitmap bounds");
        }
    }
}