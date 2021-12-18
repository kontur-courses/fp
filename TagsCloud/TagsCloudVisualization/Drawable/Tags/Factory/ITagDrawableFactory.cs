using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudVisualization.Drawable.Tags.Factory
{
    public interface ITagDrawableFactory
    {
        Result<TagDrawable> Create(Tag tag);
    }
}