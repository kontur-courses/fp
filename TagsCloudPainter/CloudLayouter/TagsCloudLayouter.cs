using ResultLibrary;
using System.Drawing;
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
    private readonly IStringSizer stringSizer;
    private readonly ITagSettings tagSettings;
    private TagsCloud cloud;

    public TagsCloudLayouter(
        ICloudSettings cloudSettings,
        IFormPointer formPointer,
        ITagSettings tagSettings,
        IStringSizer stringSizer)
    {
        this.cloudSettings = cloudSettings ?? throw new ArgumentNullException(nameof(cloudSettings));
        this.formPointer = formPointer ?? throw new ArgumentNullException(nameof(formPointer));
        this.tagSettings = tagSettings ?? throw new ArgumentNullException(nameof(tagSettings));
        this.stringSizer = stringSizer ?? throw new ArgumentNullException();
        cloud = new TagsCloud(cloudSettings.CloudCenter, []);
    }

    public Result<Rectangle> PutNextTag(Tag tag)
    {
        var tagSize = stringSizer.GetStringSize(tag.Value, tagSettings.TagFont, tag.FontSize);
        tagSize = tagSize.Then(tagSize => tagSize.Height <= 0 || tagSize.Width <= 0
        ? Result.Fail<Size>("Either width or height of rectangle size is not possitive")
        : tagSize);

        var nextRectangle = tagSize
            .Then(tagSize => formPointer.GetNextPoint().Then(point => point.GetRectangle(tagSize)))
            .Then(nextRectangle =>
            {
                while (cloud.Tags.Any(pair => pair.Item2.IntersectsWith(nextRectangle)))
                    nextRectangle = formPointer.GetNextPoint().GetValueOrThrow().GetRectangle(tagSize.GetValueOrThrow());

                return nextRectangle;
            });
        
        nextRectangle.Then(nextRectangle => cloud.AddTag(tag, nextRectangle));

        return nextRectangle;
    }

    public Result<None> PutTags(List<Tag> tags)
    {
        if (tags is null || tags.Count == 0)
            return Result.Fail<None>("Tags are empty");

        foreach (var tag in tags)
        {
            var nextTag = PutNextTag(tag);
            if (!nextTag.IsSuccess)
                return Result.Fail<None>(nextTag.Error);
        }

        return Result.Ok();
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
}