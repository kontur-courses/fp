using System.Drawing;
using System.Drawing.Imaging;
using ResultOfTask;
using TagCloudResult.Curves;

namespace TagCloudResult;

public static class Helper
{
    public static Result<ImageFormat> GetImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension.ToLower() switch
        {
            ".png" => Result.Ok(ImageFormat.Png),
            ".bmp" => Result.Ok(ImageFormat.Bmp),
            ".ico" => Result.Ok(ImageFormat.Icon),
            ".jpg" => Result.Ok(ImageFormat.Jpeg),
            ".jpeg" => Result.Ok(ImageFormat.Jpeg),
            _ => Result.Fail<ImageFormat>($"This extension '{extension}' are not supported.")
        };
    }

    public static Result<ICurve> GetCurveByName(string curveName)
    {
        return curveName.ToLower() switch
        {
            ArchimedeanSpiral.Name => Result.Ok((ICurve)new ArchimedeanSpiral()),
            _ => Result.Fail<ICurve>($"There is no '{curveName}' algorithm.")
        };
    }

    public static Result<Font> GetFont(string fontFamily, float fontSize)
    {
        if (fontSize <= 0)
            return Result.Fail<Font>("Font size cannot be less or equal to zero.");
        return Result.Of(() => new Font(fontFamily, fontSize)).RefineError("Couldn't create font");
    }

    public static Result<Size> GetSize(int width, int height)
    {
        if (width <= 0 || height <= 0)
            return Result.Fail<Size>("Size cannot be less or equal to zero.");
        return Result.Of(() => new Size(width, height)).RefineError("Couldn't create size");
    }
}