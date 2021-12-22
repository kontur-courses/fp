using ResultOf;
using System.Drawing.Imaging;

namespace TagCloud2
{
    public interface IImageFormatter
    {
        Result<ImageCodecInfo> Codec { get; }
        EncoderParameters Parameters { get; }
    }
}
