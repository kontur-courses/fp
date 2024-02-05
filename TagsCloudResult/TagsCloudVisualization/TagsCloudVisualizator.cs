using System.Drawing;
using System.Drawing.Drawing2D;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.TagProviders;
using TagsCloudVisualization.WordsAnalyzers;

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
            foreach (var tag in tags)
            {
                var font = CreateTagFont(tag);
                var tagDraw = font
                    .Then(x => graphics.MeasureString(tag.Word, x).ToSize())
                    .Then(x => layouter.PutNextRectangle(x))
                    .Then(x => graphics.DrawString(tag.Word, font.GetValueOrThrow(),
                        new SolidBrush(GetTagColor(tag)), x.Location));
                
                if (!tagDraw.IsSuccess)
                    return tagDraw;
            }
        }

        return Result.Ok();
    }

    private Result<Font> CreateTagFont(Tag tag)
    {
        return new Font(tagsSettings.Font, tagsSettings.FontSize * (float) tag.Coeff);
    }

    private Color GetTagColor(Tag tag)
    {
        return tag.Coeff > 0.75 ? tagsSettings.PrimaryColor : tag.Coeff > 0.35 ? tagsSettings.SecondaryColor 
            : tagsSettings.TertiaryColor;
    }
}