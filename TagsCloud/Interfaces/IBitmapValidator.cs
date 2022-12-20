using System.Drawing;

namespace TagsCloud.Interfaces
{
    public interface IBitmapValidator
    {
        public ResultHandler<Rectangle> ValidateNewRectangle(Rectangle rect, Bitmap bitmap);
    }
}