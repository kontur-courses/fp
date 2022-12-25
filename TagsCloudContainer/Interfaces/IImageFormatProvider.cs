using System.Drawing.Imaging;
using ResultOf;

namespace TagsCloudContainer.Interfaces;

public interface IImageFormatProvider
{
    public bool ValidateFormatName(string name);
    public Result<ImageFormat> GetFormat(string name);
    public string GetSupportedFormats();
}