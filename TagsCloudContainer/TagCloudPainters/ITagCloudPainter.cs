using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagCloudPainters
{
    public interface ITagCloudPainter
    {
        Bitmap Paint(IEnumerable<CloudTag> cloudTags);
    }
}
