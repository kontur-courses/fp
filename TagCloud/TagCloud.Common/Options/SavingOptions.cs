using System.Drawing.Imaging;
using ResultOf;

namespace TagCloud.Common.Options;

public class SavingOptions
{
    public string SavePath { get; }
    public string FileName { get; }
    public ImageFormat SavingImageFormat { get; }

    public SavingOptions(string savePath, string fileName, ImageFormat savingImageFormat)
    {
        SavePath = savePath;
        FileName = fileName;
        SavingImageFormat = savingImageFormat;
    }

    public static Result<ImageFormat> ConvertToImageFormat(string format)
    {
        return format switch
        {
            ".bmp" => ImageFormat.Bmp,
            ".jpeg" => ImageFormat.Jpeg,
            ".jpg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            _ => Result.Fail<ImageFormat>("Недопустимый формат изображения")
        };
    }

    public string GetFullSavingPath()
    {
        return SavePath + FileName + "." + SavingImageFormat.ToString().ToLower();
    }
}