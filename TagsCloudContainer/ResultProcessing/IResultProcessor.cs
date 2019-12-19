using ResultOf;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer.ResultProcessing
{
    public interface IResultProcessor
    {
        void ProcessResult(Result<Bitmap> resultOfBitmap, string filePath, ImageFormat imageFormat);
    }
}