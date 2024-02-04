using Results;

namespace TagsCloudVisualization.Settings;

public class ImageSettings
{
    public Result<int> Width { get; }
    public Result<int> Height { get; }

    public ImageSettings(int width, int height)
    {
        if (width <= 0)
            Width = Result.Fail<int>($"Width must be positive, but {width}");
        else
            Width = Result.Ok(width);
        if (height <= 0)
            Height = Result.Fail<int>($"Height must be positive, but {height}");
        else
            Height = Result.Ok(height);
    }
}