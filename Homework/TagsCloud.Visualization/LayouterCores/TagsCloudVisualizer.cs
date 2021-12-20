using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.LayoutContainer.ContainerBuilder;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization.LayouterCores
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        private readonly IDrawer drawer;
        private readonly AbstractWordsContainerBuilder wordsContainerBuilder;
        private readonly IWordsService wordsService;

        public TagsCloudVisualizer(
            IWordsService wordsService,
            AbstractWordsContainerBuilder wordsContainerBuilder,
            IDrawer drawer)
        {
            this.wordsService = wordsService;
            this.wordsContainerBuilder = wordsContainerBuilder;
            this.drawer = drawer;
        }

        public Result<Image> GenerateImage(IWordsProvider wordsProvider)
        {
            return wordsService.GetWords(wordsProvider)
                .Then(words => wordsContainerBuilder.AddWords(words,
                    words.Min(x => x.Count),
                    words.Max(x => x.Count)))
                .Then(container => container.Build())
                .Then(container => drawer.Draw(container));
        }
    }
}