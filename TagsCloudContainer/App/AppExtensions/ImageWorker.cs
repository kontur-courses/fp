using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App.AppExtensions;

public static class ImageWorker
{
    private static string ParentDirectoryPath => 
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Images";

    private enum ImageFormat
    {
        Png,
        Jpeg,
    }

    public static Result<None> SaveImage(Bitmap image, Settings settings)
    {
        return GetPathForSaveImage(settings)
            .Then(image.Save);
    }

    private static Result<string> GetPathForSaveImage(Settings settings)
    {
        var randomImagesSuffix = new Random().Next(1, 1000);

        return GetImageFormat(settings.ImageFormat)
            .Then(format => Path.Combine(ParentDirectoryPath, $"Rectangle{randomImagesSuffix}.{format}"));
    }

    private static Result<ImageFormat> GetImageFormat(string imageFormat)
    {
        return Result.Of(() =>
        {
            return imageFormat.ToLower() switch
            {
                "png" => ImageFormat.Png,
                "jpeg" => ImageFormat.Jpeg,
                _ => throw new FormatException($"Неверный формат изображения: {imageFormat}")
            };
        });
    }
}