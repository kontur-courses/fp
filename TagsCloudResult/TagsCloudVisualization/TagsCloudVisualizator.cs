using System.Drawing;
using System.Drawing.Drawing2D;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.TagProviders;

namespace TagsCloudVisualization;

public class TagsCloudVisualizator
{
    private readonly ITagsCloudLayouter layouter;
    private readonly ITagProvider tagProvider;
    private readonly IImageHolder imageHolder;
    private readonly TagsSettings tagsSettings;
    
    public TagsCloudVisualizator(ITagsCloudLayouter layouter, IImageHolder imageHolder, ITagProvider tagProvider, TagsSettings tagsSettings)
    {
        this.tagProvider = tagProvider;
        this.layouter = layouter;
        this.tagsSettings = tagsSettings;
        this.imageHolder = imageHolder;
    }

    public Result<None> DrawTagsCloud()
    {
        return imageHolder.StartDrawing()
            .Then(DrawAndSave)
            .Then(_ => imageHolder.UpdateUi());
    }

    private Result<GraphicsState> DrawAndSave(Graphics graphics)
    {
        using (graphics)
        {
            return FillBackground(graphics)
                .Then(_ => tagProvider.GetTags())
                .Then(tags => DrawTags(graphics, tags))
                .Then(_ => graphics.Save());
        }
    }

    private Result<None> FillBackground(Graphics graphics)
    {
        graphics.Clear(tagsSettings.BackgroundColor);
        return Result.Ok();
    }

    private Result<None> DrawTags(Graphics graphics, IEnumerable<Tag> tags)
    {
        using (layouter)
        {
            return tags.Select(tag => Result.Of( () => GetTagSize(tag, graphics))
                .Then(tagSize => layouter.PutNextRectangle(tagSize))
                .Then(rectangle => DrawTag(tag, graphics, rectangle.Location)))
                .FirstOrDefault(x => !x.IsSuccess);
        }
    }

    private Size GetTagSize(Tag tag, Graphics graphics)
    {
        return graphics.MeasureString(tag.Word, CreateTagFont(tag)).ToSize();
    }

    private void DrawTag(Tag tag, Graphics graphics, Point position)
    {
        graphics.DrawString(tag.Word, CreateTagFont(tag), GetTagBrush(tag.Coeff), position);
    }

    private Font CreateTagFont(Tag tag)
    {
        return new Font(tagsSettings.Font, tagsSettings.FontSize * (float) tag.Coeff);
    }

    private SolidBrush GetTagBrush(double tagCoeff)
    {
        const double popularTagThreshold = 0.75;
        const double commonTagThreshold = 0.35;
        
        return tagCoeff switch
        {
            > popularTagThreshold => new SolidBrush(tagsSettings.PrimaryColor),
            > commonTagThreshold => new SolidBrush(tagsSettings.SecondaryColor),
            _ => new SolidBrush(tagsSettings.TertiaryColor)
        };
    }
}
