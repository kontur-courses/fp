using Results;

namespace TagsCloudVisualization.Settings;

public class ImageSettings : ISettings
{
    public int Width { get; }
    public int Height { get; }

    public ImageSettings(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public Result<bool> Check()
    {
        if (Width <= 0)
            return Result.Fail<bool>($"Width must be positive, but {Width}");
        else if (Height <= 0)
            return Result.Fail<bool>($"Height must be positive, but {Height}");
        return Result.Ok(true);

    }
}