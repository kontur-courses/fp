using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.TagsCloudLayouter;

namespace TagsCloudContainer.Infrastructure;
public class CloudTagCreator
{
    private readonly ICloudLayouter layouter;
    private readonly Settings.Settings settings;

    public CloudTagCreator(ICloudLayouter layouter,
        Settings.Settings settings)
    {
        this.layouter = layouter;
        this.settings = settings;
    }

    public Result<IEnumerable<CloudTag>> CreateCloudTags(IEnumerable<PaintedTag> tags)
    {
        var tagsList = tags.ToList();
        if (tagsList.Count < 1)
            return Result.Fail<IEnumerable<CloudTag>>("No tags received!");
        var averageFrequency = tagsList.Select(tag => tag.Frequency)
            .Sum() / tagsList.Count;

        return Result.Ok(GetCloudTags(tagsList, averageFrequency));
    }

    private IEnumerable<CloudTag> GetCloudTags(List<PaintedTag> tagsList, double averageFrequency)
    {
        foreach (var tag in tagsList)
        {
            var fontSize = (float) (tag.Frequency / averageFrequency) * settings.Font.Size;
            var label = new Label {AutoSize = true};
            label.Font = new Font(settings.Font.FontFamily, fontSize, settings.Font.Style);
            label.Text = tag.Text;
            var size = label.GetPreferredSize(settings.ImageSize);
            var result = layouter.PutNextRectangle(size);
            if (!result.IsSuccess)
                continue;
            yield return new CloudTag(tag, label, result.Value);
        }

        layouter.Reset();
    }
}

