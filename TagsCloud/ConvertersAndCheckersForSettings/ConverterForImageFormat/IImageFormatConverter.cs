using System.Drawing.Imaging;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat
{
    public interface IImageFormatConverter
    {
        ImageFormat ConvertToImageFormat(string imageFormatFromString);
    }
}