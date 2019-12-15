using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using TagsCloud.Layouters;
using TagsCloud.Renderers;

namespace TagsCloud
{
    public class TagsCloudGenerator
    {
        private readonly ITagsCloudLayouter layouter;
        private readonly ITagsCloudRenderer renderer;

        public TagsCloudGenerator(ITagsCloudLayouter layouter, ITagsCloudRenderer renderer)
        {
            this.layouter = layouter;
            this.renderer = renderer;
        }

        public Result<Image> GenerateCloud(ImmutableList<string> words)
        {
            return Result.Of(() => DetermineTags(words))
                .Then(tags => GetLayout(tags))
                .Then(layout => renderer.Render(layout))
                .Then(image => TagCloudImage = image)
                .RefineError("Can't generate tags cloud");
        }

        private ImmutableList<(string Tag, int Rate)> DetermineTags(ImmutableList<string> words)
        {
            return ImmutableList<(string Tag, int Rate)>.Empty
                .AddRange(words.GroupBy(word => word).Select(group => (group.Key, group.Count())));
        }

        private Result<ImmutableList<LayoutItem>> GetLayout(ImmutableList<(string Tag, int Rate)> tags)
        {
            return Result.Of(() => tags.ConvertAll(t => new LayoutItem(new Rectangle(Point.Empty, Size.Empty), t.Tag, t.Rate)))
                .Then(layoutItems => renderer.CalcTagsRectanglesSizes(layoutItems))
                .Then(sizedItems => layouter.ReallocItems(sizedItems))
                .RefineError("Can't create layout.");
        }

        public Image TagCloudImage { get; private set; }
    }
}
