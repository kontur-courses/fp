using System.Collections.Generic;
using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.Templates.Colors;

namespace TagCloud.Templates;

internal class TemplateCreator : ITemplateCreator
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

    public TemplateCreator(TemplateConfiguration templateConfiguration, IFontSizeCalculator fontSizeCalculator,
        IColorGenerator colorGenerator, ICloudLayouter cloudLayouter)
        : this(templateConfiguration.FontFamily, templateConfiguration.BackgroundColor, templateConfiguration.Size,
            fontSizeCalculator, colorGenerator, cloudLayouter)
    {
    }

    public Result<ITemplate> GetTemplate(IEnumerable<string> words)
    {
        var template = new Template { ImageSize = size };
        var wordToSize = fontSizeCalculator.GetFontSizes(words);
        var bitmap = new Bitmap(1, 1);
        var g = Graphics.FromImage(bitmap);
        foreach (var (word, fontSize) in wordToSize)
        {
            var font = new Font(fontFamily, fontSize);
            var wordParameter = g.MeasureString(word, font).AsResult()
                .Then(cloudLayouter.PutNextRectangle)
                .Then(r => new WordParameter(word, font, colorGenerator.GetColor(word), r));
            if (!wordParameter.IsSuccess)
                return Result.Fail<ITemplate>($"Error on handling {word}");
            if (!template.TryAdd(wordParameter.Value))
                return Result.Fail<ITemplate>("Tag cloud did not fit on the image of the given size");
        }

        template.BackgroundColor = backgroundColor;
        return template;
    }
}