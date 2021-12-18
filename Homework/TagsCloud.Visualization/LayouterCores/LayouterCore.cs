using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.LayoutContainer.ContainerBuilder;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization.LayouterCores
{
    public class LayouterCore : ILayouterCore
    {
        private readonly IDrawer drawer;
        private readonly AbstractWordsContainerBuilder wordsContainerBuilder;
        private readonly IWordsService wordsService;

        public LayouterCore(
            IWordsService wordsService,
            AbstractWordsContainerBuilder wordsContainerBuilder,
            IDrawer drawer)
        {
            this.wordsService = wordsService;
            this.wordsContainerBuilder = wordsContainerBuilder;
            this.drawer = drawer;
        }

        public Image GenerateImage(IWordsReadService wordsReadService)
        {
            var parsedWords = wordsService.GetWords(wordsReadService);

            var maxCount = parsedWords.Max(x => x.Count);
            var minCount = parsedWords.Min(x => x.Count);

            var wordsContainer = wordsContainerBuilder
                .AddWords(parsedWords, minCount, maxCount)
                .Build();

            return drawer.Draw(wordsContainer);
        }
    }
}