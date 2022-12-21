using System.Drawing.Imaging;
using TagsCloudVisualization;

namespace TagsCloudVisualizationConsole;

public static class AppOptionsValidator
{
    public static Result<ArgsOptions> ValidatePathsInOptions(ArgsOptions argsOptions)
    {
        if (!File.Exists(argsOptions.PathToTextFile))
            return Result.Fail<ArgsOptions>($"File \"{argsOptions.PathToTextFile}\" does not exist. Check or create file.");

        if (!Directory.Exists(argsOptions.DirectoryToSaveFile))
            return Result.Fail<ArgsOptions>($"Directory \"{argsOptions.DirectoryToSaveFile}\" does not exist. Check path or create directory.");

        if (string.IsNullOrEmpty(argsOptions.SaveFileName))
            return Result.Fail<ArgsOptions>($"Save file name empty. Pass file name to save result image by SaveFileName parameter");

        return argsOptions;
    }


    public static Result<ImageFormat> GetImageFormat(string? format)
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