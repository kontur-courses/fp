using Mono.Options;
using ResultExtensions;
using ResultOf;
using System.Drawing;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class StyleProvider : ICliSettingsProvider
{
    private const string defaultFont = "Comic Sans MS";
    private const int defaultSize = 50;
    private static readonly Color defaultColor = Color.White;

    public string FontFamilyName { get; private set; } = defaultFont;
    public double MinSize { get; private set; } = defaultSize;
    public Color BrushColor { get; private set; } = defaultColor;

    public Result State { get; private set; } = Result.Ok();
    public Style GetStyle(ITag tag)
    {
        var font = new Font(new FontFamily(FontFamilyName), (float)((1 + tag.RelativeSize) * MinSize));
        return new(font, new SolidBrush(BrushColor));
    }

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            { "font-family=", $"Sets the font family name for tags. Defaults to {defaultFont}", v => FontFamilyName = v },
            { "min-size=", $"Sets the min size for tags. Defaults to {defaultSize}", v => State = ParseDouble(v).Then(size => MinSize = size) },
            { "color=", $"Sets the color for tags. Defaults to {defaultColor.Name}",  
                v => State = Result.Of(() => ColorTranslator.FromHtml(v)).ReplaceError(_ => $"Could not parse {v} as {nameof(Color)}").Then(c => BrushColor = c) },
        };

        return options;
    }

    private static Result<double> ParseDouble(string v)
    {
        return double.TryParse(v, out var r) ? r : Result.Fail<double>($"Could not parse {v} as {nameof(Double)}");
    }
}
