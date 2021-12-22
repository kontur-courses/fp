using System.Drawing;

namespace TagCloud.Templates;

public class TemplateConfiguration
{
    public FontFamily FontFamily { get; }

    public Color BackgroundColor { get; }

    public Size Size { get; }
        
    public TemplateConfiguration(FontFamily fontFamily, Color backgroundColor, Size size)
    {
        FontFamily = fontFamily;
        BackgroundColor = backgroundColor;
        Size = size;
    }
}