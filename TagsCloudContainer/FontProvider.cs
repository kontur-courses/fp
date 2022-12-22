using System.Drawing;

namespace TagsCloudContainer;

public class FontProvider
{
    public Result<Font> TryGetFont(string fontName, float fontSize)
    {
        var font = new Font(fontName, fontSize);
        
        //Проверка на доступность имени
        //Если фонт не может получить шрифт по имени, то он ставит дефолтный   
        return font.Name != fontName 
            ? Result.Fail<Font>($"No font with this name found: {fontName}")
            : font.Ok();
    }
}