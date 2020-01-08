using System.Collections.Generic;
using System.Drawing.Imaging;
using ResultOf;

namespace TagCloud.Services
{
    public interface IFormatReader
    {
        Result<ImageFormat> ReadFormat(Dictionary<string, ImageFormat> availableFormats);
    }
}