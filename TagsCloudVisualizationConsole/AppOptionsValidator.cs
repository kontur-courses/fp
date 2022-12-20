using System.Drawing.Imaging;
using TagsCloudVisualization;

namespace TagsCloudVisualizationConsole;

public static class AppOptionsValidator
{
    public static Result<None?> ValidatePathsInOptions(ArgsOptions? argsOptions)
    {
        if (argsOptions == null)
            return Result.Fail<None?>($"{nameof(argsOptions)} is null");

        if (!File.Exists(argsOptions.PathToTextFile))
            return Result.Fail<None?>($"{argsOptions.PathToTextFile} does not exist");

        if (!Directory.Exists(argsOptions.DirectoryToSaveFile))
            return Result.Fail<None?>($"{argsOptions.DirectoryToSaveFile} does not exist");

        if (string.IsNullOrEmpty(argsOptions.SaveFileName))
            return Result.Fail<None?>($"Save file name empty");

        return Result.Ok();
    }


    public static Result<ImageFormat> GetImageFormat(string format)
    {
        return format.ToLower() switch
        {
            "png" => ImageFormat.Png,
            "bmp" => ImageFormat.Bmp,
            "emf" => ImageFormat.Emf,
            "gif" => ImageFormat.Gif,
            "icon" => ImageFormat.Icon,
            "jpeg" => ImageFormat.Jpeg,
            "tiff" => ImageFormat.Tiff,
            _ => Result.Fail<ImageFormat>("ImageFormat unexpected")
        };
    }
}