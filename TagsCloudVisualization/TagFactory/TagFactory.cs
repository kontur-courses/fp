using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.ColorGenerator;
using TagsCloudVisualization.Drawer;
using TagsCloudVisualization.FontSettings;

namespace TagsCloudVisualization.TagFactory;

public class TagFactory : ITagFactory
{
    private readonly ICloudLayouter layouter;
    private readonly IColorGenerator colorGenerator;
    private readonly IFontSettingsProvider fontSettingsProvider;

    public TagFactory(ICloudLayouter layouter, IColorGenerator colorGenerator,
        IFontSettingsProvider fontSettingsProvider)
    {
        this.layouter = layouter;
        this.colorGenerator = colorGenerator;
        this.fontSettingsProvider = fontSettingsProvider;
    }

    public Result<TagImage> Create(Tag tag)
    {
        return fontSettingsProvider.GetSettings()
            .Then(settings => settings.Size * tag.Weight)
            .Then(height => new SizeF(height * tag.Text.Length, height))
            .Then(size =>
                new TagImage(tag, layouter.PutNextRectangle(size.ToSize()), fontSettingsProvider, colorGenerator))
            .RefineError("Failed to create tag");
    }
}