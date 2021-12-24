using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.Templates.Colors;

namespace TagCloud.Templates;

public class TemplateCreator : ITemplateCreator
{
    private readonly FontFamily fontFamily;
    private readonly Color backgroundColor;
    private readonly Size size;
    private readonly IFontSizeCalculator fontSizeCalculator;
    private readonly IColorGenerator colorGenerator;
    private readonly ICloudLayouter cloudLayouter;

    public TemplateCreator(FontFamily fontFamily, Color backgroundColor, Size size,
        IFontSizeCalculator fontSizeCalculator, IColorGenerator colorGenerator, ICloudLayouter cloudLayouter)
    {
        this.fontFamily = fontFamily;
        this.backgroundColor = backgroundColor;
        this.size = size;
        this.fontSizeCalculator = fontSizeCalculator;
        this.colorGenerator = colorGenerator;
        this.cloudLayouter = cloudLayouter;
    }

    public TemplateCreator(TemplateConfiguration templateConfiguration, IFontSizeCalculator fontSizeCalculator, IColorGenerator colorGenerator, ICloudLayouter cloudLayouter)
        : this(templateConfiguration.FontFamily, templateConfiguration.BackgroundColor, templateConfiguration.Size,
            fontSizeCalculator, colorGenerator, cloudLayouter)
    {
    }

    public ITemplate GetTemplate(IEnumerable<string> words)
    {
        var template = new Template { ImageSize = size };
        var wordToSize = fontSizeCalculator.GetFontSizes(words);
        var bitmap = new Bitmap(1, 1);
        var g = Graphics.FromImage(bitmap);
        foreach (var (word, fontSize) in wordToSize)
        {
            var font = new Font(fontFamily, fontSize);
            var wordParameter = new WordParameter(word, font, colorGenerator.GetColor(word));
            var wordSize = g.MeasureString(word, font);
            wordParameter.WordRectangleF = cloudLayouter.PutNextRectangle(wordSize);
            if (!template.TryAdd(wordParameter))
                throw new Exception("Tag cloud did not fit on the image of the given size");
        }

        template.BackgroundColor = backgroundColor;
        return template;
    }
}