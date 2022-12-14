using System.Drawing;
using TagCloud.Infrastructure;

namespace TagCloud;

public class FontProvider
{
    private readonly string _name;

    public FontProvider(string name)
    {
        _name = name;
        if (name == null)
            _name = "Arial";
    }

    public Result<Font> GetFont()
    {
        var font = new Font(_name, 1);
        if (font.Name != _name)
            return new Result<Font>($"Coult not find font with name \"{_name}\"");

        return font;
    }
}