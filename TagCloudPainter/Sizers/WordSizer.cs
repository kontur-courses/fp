using System.Drawing;
using TagCloudPainter.Common;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Sizers;

public class WordSizer : IWordSizer
{
    private readonly ImageSettings settings;

    public WordSizer(IImageSettingsProvider provider)
    {
        settings = provider.ImageSettings;
    }

    public Result<Size> GetTagSize(string word, int count)
    {
        if (string.IsNullOrWhiteSpace(word))
            return Result.Fail<Size>("word is null or white space");

        if (count <= 0)
            return Result.Fail<Size>("the word does not appear in the text");

        if (settings == null)
            return Result.Fail<Size>("settings could not be read");

        var width = word.Length * (int)(settings.Font.Size + 1) + 3 * (count - 1);
        var height = (settings.Size.Height + settings.Size.Width) / 40 + 2 * (count - 1);
        return new Size(width, height);
    }
}