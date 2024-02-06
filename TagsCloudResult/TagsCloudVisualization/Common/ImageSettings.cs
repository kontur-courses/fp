using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.Common;

public class ImageSettings : ISettings<ImageSettings>
{
    public int Width { get; init; } = 1000;
    public int Height { get; init; } = 1000;

    public Result<ImageSettings> Validate()
    {
        return ValidateWidthAndHeight(this);
    }
    
    private static Result<ImageSettings> ValidateWidthAndHeight(ImageSettings settings)
    {
        return Result.Validate(settings, x => x is {Height: > 0, Width: > 0},
            "Ширина и высота изображения не могут быть меньше или равны нулю. Пожалуйста, введите корректные настройки.");
    }
}
