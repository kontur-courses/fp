using Results;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.ImageSavers;

public class ImageSaver : IImageSaver
{
    private readonly ImageSaverSettings settings;
    
    public ImageSaver(ImageSaverSettings fileSettings) 
    {
        settings = fileSettings;
    }

    public Result<None> SaveImage(Bitmap bitmap)
    {
        if (!IsFileNameValid(settings.FileName))
        {
            return Result.Fail<None>($"filename {settings.FileName} is incorrect");
        }
        if (!IsPathValid(settings.PathToSaveDirectory))
        {
            return Result.Fail<None>($"path {settings.PathToSaveDirectory} is incorrect");
        }

        if (!Directory.Exists(settings.PathToSaveDirectory))
        {
            Directory.CreateDirectory(settings.PathToSaveDirectory);
        }
        var format = GetImageFormat(settings.FileFormat);
        if (!format.IsSuccess)
            return Result.Fail<None>(format.Error);
        bitmap.Save(Path.Combine(settings.PathToSaveDirectory, settings.FileName + "." + settings.FileFormat),
            format.Value);
        return Result.Ok();
    }

    public static bool IsFileNameValid(string fileName)
    {
        return !(fileName == null || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1);
    }

    public static bool IsPathValid(string filePath)
    {
        return !(filePath.IndexOfAny(Path.GetInvalidPathChars()) != -1);

    }

    public static Result<ImageFormat> GetImageFormat(string format)
    {
        var imageFormatConverter = new ImageFormatConverter();
        try
        {
            var imageFormat = imageFormatConverter.ConvertFromString(format);
            return Result.Ok((ImageFormat)imageFormat);
        }
        catch (FormatException)
        {
            return Result.Fail<ImageFormat>($"Can't convert format from this string {format}");
        }
    }
}