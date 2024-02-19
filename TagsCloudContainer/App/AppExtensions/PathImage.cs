using System.Drawing.Imaging;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App.AppExtensions;

public class PathImage
{
    public string GetPathForSaveImage(Settings settings)
    {
        var imagesDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Images";
        var randomImagesSuffix = new Random().Next(1, 1000);
        var imageFormat = GetImageFormat(settings.ImageFormat).GetValueOrThrow();
        var path = Path.Combine(imagesDirectory, $"Rectangle{randomImagesSuffix}.{imageFormat}");
        return path;
    }
    
    private Result<ImageFormat> GetImageFormat(string imageFormat)
    {
        return imageFormat.ToLower() switch
        {
            "png" =>  ImageFormat.Png.Ok(),
            "jpeg" => ImageFormat.Jpeg.Ok(),
            _ => Result.Fail<ImageFormat>($"Неверный формат изображения: {imageFormat}")
        };
    }
}