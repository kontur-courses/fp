using ResultOf;
using TagsCloudContainer.TagsClouds;
using TagsCloudContainer.Visualisation.Coloring;

namespace TagsCloudContainer.Visualisation
{
    public interface ITagsCloudRenderer
    {
        Result<None> RenderIntoFile(ImageSettings imageSettings, IColorManager colorManager, ITagsCloud tagsCloud);
    }
}