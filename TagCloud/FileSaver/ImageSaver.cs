using System.Drawing;
using ResultOf;

namespace TagCloud.FileSaver;

public class ImageSaver : ISaver
{
    private List<string> supportedFormats = new() { "png", "jpg", "jpeg", "bmp", "gif" };

    public Result<None> Save(Bitmap bitmap, string outputPath, string imageFormat)
    {
        if (!supportedFormats.Contains(imageFormat))
            return Result.Fail<None>($"{imageFormat} output format is not supported");

        using (bitmap)
        {
            bitmap.Save($"{outputPath}.{imageFormat}");
        }

        return Result.Ok();
    }
}