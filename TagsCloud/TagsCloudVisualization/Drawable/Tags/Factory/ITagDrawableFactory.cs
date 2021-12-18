using ResultMonad;

namespace TagsCloudVisualization.Drawable.Tags.Factory
{
    public interface ITagDrawableFactory
    {
        Result<TagDrawable> Create(Tag tag);
    }
}