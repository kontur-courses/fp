using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Parsers
{
    public class ImageFormatParser : IImageFormatParser
    {
        private const BindingFlags bindingAttributes =
            BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase;

        public Result<ImageFormat> Parse(string value)
        {
            return GetImageFormat(value)
                .RefineError($"Available formats: {GetAvailableFormats()}");
        }

        private static Result<ImageFormat> GetImageFormat(string value)
        {
            var format = (ImageFormat?)typeof(ImageFormat)
                .GetProperty(value, bindingAttributes)
                ?.GetValue(null);

            return format != null
                ? Result.Ok(format)
                : Result.Fail<ImageFormat>("Can't find image format");
        }

        private static string GetAvailableFormats()
        {
            return string.Join(", ",
                typeof(ImageFormat).GetProperties(bindingAttributes).Select(propInfo => propInfo.Name));
        }
    }
}