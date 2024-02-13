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
    }

    public TagsCloud PutTags(List<Tag> tags)
    {
        var cloud = new TagsCloud(cloudSettings.CloudCenter, []);
        var tagSizes = tags?.Select(tag => tagSizer.GetTagSize(tag, tagSettings.TagFont));
        var tagsRectangles = tagSizes?.Select(tagSize => tagSize.Then(size => 
        GetTagRectangle(size, cloud.Tags.Select(pair => pair.Item2))));
        tagsRectangles?.Zip(tags ?? [], (rectangle, tag) => rectangle.Then(rectangle => cloud.AddTag(tag, rectangle)))
            .ToList();

        return cloud;
    }

    public void Reset()
    {
        formPointer.Reset();
    }

    private Result<Rectangle> GetTagRectangle(Size tagSize, IEnumerable<Rectangle> rectangles)
    {
        var tagRectangle = formPointer.GetNextPoint().Then(point => point.GetRectangle(tagSize))
            .Then(tagRectangle =>
            {
                while (rectangles.Any(rectangle => rectangle.IntersectsWith(tagRectangle)))
                    tagRectangle = formPointer.GetNextPoint().GetValueOrThrow()
                        .GetRectangle(tagSize);

                return tagRectangle;
            });

        return tagRectangle;
    }
}