using System.Drawing;

namespace TagsCloudContainer.BitmapSaver
{
    public interface IBitmapSaver
    {
        public Result<None> Save(Bitmap bmp, string fullPathWithExt);
    }
}