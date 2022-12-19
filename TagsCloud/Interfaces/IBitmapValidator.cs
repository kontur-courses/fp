using System.Drawing;

namespace TagsCloud.Interfaces
{
    public interface IBitmapValidator
    {
        public Result<Rectangle> ValidateNewRectangle(Rectangle rect, Bitmap bitmap);
    }
}