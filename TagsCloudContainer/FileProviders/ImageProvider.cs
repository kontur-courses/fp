using SixLabors.ImageSharp;

namespace TagsCloudContainer.FileProviders;

public class ImageProvider: IImageProvider
{
    public Result<None> SaveImage(Image image, string filePath)
    {
        try
        {
            image.Save(filePath);
        }
        catch (Exception e)
        {
            return Result.Fail<None>("Ошибка при сохранении изображения. " + e.Message); 
        }

        return Result.Ok();
    }
}