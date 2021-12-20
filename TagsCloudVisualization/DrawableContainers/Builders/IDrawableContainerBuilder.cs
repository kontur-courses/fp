using System.Collections.Generic;

namespace TagsCloudVisualization.DrawableContainers.Builders
{
    public interface IDrawableContainerBuilder
    {

        void AddTags(IEnumerable<Tag> tags);
        IDrawableContainer Build();
    }
}