using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer;

public class FontProvider
{
    public Result<Font> TryGetFont(string fontName, float fotSize)
    {
        var font = new Font(fontName, fotSize);
        return font.Name != fontName
            ? Result.Fail<Font>($"Шрифт с таким именем не найден в системе: {fontName}")
            : font.Ok();
    }
}
