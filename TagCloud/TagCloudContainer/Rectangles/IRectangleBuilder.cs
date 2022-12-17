using TagCloudContainer.TagsWithFont;

namespace TagCloudContainer.Rectangles
{
    public interface IRectangleBuilder
    {
        IEnumerable<Result<SizeTextRectangle>> GetRectangles(IEnumerable<ITag> tags);
    }
}
