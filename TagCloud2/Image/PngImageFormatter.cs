using ResultOf;
using System.Drawing.Imaging;

namespace TagCloud2.Image
{
    public class PngImageFormatter : IImageFormatter
    {
        public Result<ImageCodecInfo> Codec => ImageFormatterHelper.GetEncoderInfo("image/png");

        public EncoderParameters Parameters => null;
    }
}
