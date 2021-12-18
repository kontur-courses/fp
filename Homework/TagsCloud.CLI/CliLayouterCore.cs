using System;
using TagsCloud.Visualization.ImagesSavior;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Words
{
    public class CliLayouterCore
    {
        private readonly IImageSavior imageSavior;
        private readonly ILayouterCore layouterCore;
        private readonly IWordsReadService wordsReadService;

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
            layouterCore.GenerateImage(wordsReadService)
                .OnFail(Console.WriteLine)
                .Then(x => imageSavior.Save(x))
                .Then(x => x.Dispose());
        }
    }
}