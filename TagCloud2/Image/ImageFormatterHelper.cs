using ResultOf;
using System.Drawing.Imaging;

namespace TagCloud2.Image
{
    public static class ImageFormatterHelper
    {
        public static Result<ImageCodecInfo> GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (var j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }

            return Result.Fail<ImageCodecInfo>("No such codec");
        }
    }
}
