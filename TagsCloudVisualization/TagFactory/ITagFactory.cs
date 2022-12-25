using TagsCloudVisualization.Drawer;

namespace TagsCloudVisualization.TagFactory;

public interface ITagFactory
{
    Result<TagImage> Create(Tag tag);
}