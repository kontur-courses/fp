using System.Drawing;
using TagsCloudContainer.DrawRectangle;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App.AppExtensions;

public class DrawImage
{
    public static Result<Bitmap> CreateImage(Settings settings, IDraw draw)
    {
        var words = new ProcessWord().GetProcessWords(settings.File, settings.BoringWordsFileName, settings);
        var bitmap = draw.CreateImage(words).OnFail(ProcessWord.Exit);
        return bitmap;
    }
}