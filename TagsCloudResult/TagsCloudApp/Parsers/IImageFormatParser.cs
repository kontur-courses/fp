using System.Drawing.Imaging;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Parsers
{
    public interface IImageFormatParser
    {
        Result<ImageFormat> Parse(string value);
    }
}