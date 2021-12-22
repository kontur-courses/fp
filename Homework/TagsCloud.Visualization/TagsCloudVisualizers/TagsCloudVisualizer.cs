using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.LayoutContainer.ContainerBuilder;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TagsCloudVisualizers
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        private readonly IDrawer drawer;
        private readonly AbstractTagsContainerBuilder tagsContainerBuilder;
        private readonly IWordsService wordsService;

        public TagsCloudVisualizer(
            IWordsService wordsService,
            AbstractTagsContainerBuilder tagsContainerBuilder,
            IDrawer drawer)
        {
            this.wordsService = wordsService;
            this.tagsContainerBuilder = tagsContainerBuilder;
            this.drawer = drawer;
        }

        public Result<Image> GenerateImage(ITextProvider textProvider)
        {
            return wordsService.GetWords(textProvider)
                .Then(words => tagsContainerBuilder.AddWords(words,
                    words.Min(x => x.Count),
                    words.Max(x => x.Count)))
                .Then(container => container.Build())
                .Then(container => drawer.Draw(container));
        }
    }
}