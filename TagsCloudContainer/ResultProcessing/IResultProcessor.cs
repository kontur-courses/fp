using System.Drawing;
using System.Drawing.Imaging;
using ResultOf;

namespace TagsCloudContainer.ResultProcessing
{
    public interface IResultProcessor
    {
        void ProcessResult(Result<Bitmap> resultOfBitmap, string filePath, ImageFormat imageFormat);
    }
}