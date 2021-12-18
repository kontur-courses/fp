using TagsCloud.Visualization.ImagesSavior;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Words
{
    public class CliLayouterCore
    {
        private readonly IImageSavior imageSavior;
        private readonly IWordsReadService wordsReadService;
        private readonly ILayouterCore layouterCore;

        public CliLayouterCore(
            ILayouterCore layouterCore,
            IImageSavior imageSavior,
            IWordsReadService wordsReadService)
        {
            this.layouterCore = layouterCore;
            this.imageSavior = imageSavior;
            this.wordsReadService = wordsReadService;
        }

        public void Run()
        {
            using var image = layouterCore.GenerateImage(wordsReadService);

            imageSavior.Save(image);
        }
    }
}