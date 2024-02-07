using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.Common;

public record TagsSettings : ISettings<TagsSettings>
{
    [TypeConverter(typeof(FontConverter.FontNameConverter))]
    public string Font { get; init; } = "Arial";
    public int FontSize { get; init; } = 42;
    public Color PrimaryColor { get; init; } = Color.Yellow;
    public Color SecondaryColor { get; init; } = Color.Red;
    public Color TertiaryColor { get; init; } = Color.Aquamarine;
    public Color BackgroundColor { get; init; } = Color.DarkBlue;
    
    public Result<TagsSettings> Validate()
    {
        return ValidateFont(this)
            .Then(ValidateFontSize);
    }
    
    private static Result<TagsSettings> ValidateFont(TagsSettings settings)
    {
        var fontsCollection = new InstalledFontCollection();
        return Result.Validate(settings, x => fontsCollection.Families.Select(fontFamily => fontFamily.Name).Contains(x.Font),
            Resources.TagsSettings_ValidateFont_Fail);
    }

    private static Result<TagsSettings> ValidateFontSize(TagsSettings settings)
    {
        return Result.Validate(settings, x => x.FontSize > 0,
            Resources.TagsSettings_ValidateFontSize_Fail);
    }
}
