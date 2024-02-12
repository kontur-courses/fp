using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Extensions;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.Tag;
using TagsCloudPainter.Sizer;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.CloudLayouter;

public class TagsCloudLayouter : ICloudLayouter
{
    private readonly ICloudSettings cloudSettings;
    private readonly IFormPointer formPointer;
    private readonly ITagSettings tagSettings;
    private readonly ITagSizer tagSizer;
    private TagsCloud cloud;

    public TagsCloudLayouter(
        ICloudSettings cloudSettings,
        IFormPointer formPointer,
        ITagSettings tagSettings,
        ITagSizer tagSizer)
    {
        this.cloudSettings = cloudSettings ?? throw new ArgumentNullException(nameof(cloudSettings));
        this.formPointer = formPointer ?? throw new ArgumentNullException(nameof(formPointer));
        this.tagSettings = tagSettings ?? throw new ArgumentNullException(nameof(tagSettings));
        this.tagSizer = tagSizer ?? throw new ArgumentNullException(nameof(tagSizer));
        cloud = new TagsCloud(cloudSettings.CloudCenter, []);
    }

    public Result<Rectangle> PutNextTag(Tag tag)
    {
        var tagSize = tagSizer.GetTagSize(tag, tagSettings.TagFont);
        var nextRectangle = tagSize.Then(GetTagRectangle);

        nextRectangle.Then(nextRectangle => cloud.AddTag(tag, nextRectangle));

        return nextRectangle;
    }

    public TagsCloud PutTags(List<Tag> tags)
    {
        var cloud = new TagsCloud(this.cloud.Center, this.cloud.Tags);
        var tagSizes = tags?.Select(tag => tagSizer.GetTagSize(tag, tagSettings.TagFont));
        var tagsRectangles = tagSizes?.Select(tagSize => tagSize.Then(GetTagRectangle));
        tagsRectangles?.Zip(tags ?? [], (rectangle, tag) => rectangle.Then(rectangle => cloud.AddTag(tag, rectangle)))
            .ToList();

        return cloud;
    }

    public TagsCloud GetCloud()
    {
        return new TagsCloud(cloud.Center, cloud.Tags);
    }

    public void Reset()
    {
        formPointer.Reset();
        cloud = new TagsCloud(cloudSettings.CloudCenter, []);
    }

    private Result<Rectangle> GetTagRectangle(Size tagSize)
    {
        var tagRectangle = formPointer.GetNextPoint().Then(point => point.GetRectangle(tagSize))
            .Then(tagRectangle =>
            {
                while (cloud.Tags.Any(pair => pair.Item2.IntersectsWith(tagRectangle)))
                    tagRectangle = formPointer.GetNextPoint().GetValueOrThrow()
                        .GetRectangle(tagSize);

                return tagRectangle;
            });

        return tagRectangle;
    }
}