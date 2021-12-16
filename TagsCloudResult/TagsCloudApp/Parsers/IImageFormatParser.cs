using System.Drawing.Imaging;
using TagsCloudContainer;

namespace TagsCloudApp.Parsers
{
    public interface IImageFormatParser
    {
        Result<ImageFormat> Parse(string value);
    }
}