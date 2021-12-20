using System;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.ImagesSavers;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Words
{
    public class CliLayouterCore
    {
        private readonly IImageSaver imageSaver;
        private readonly ILayouterCore layouterCore;
        private readonly IWordsReadService wordsReadService;

        public CliLayouterCore(
            ILayouterCore layouterCore,
            IImageSaver imageSaver,
            IWordsReadService wordsReadService)
        {
            this.layouterCore = layouterCore;
            this.imageSaver = imageSaver;
            this.wordsReadService = wordsReadService;
        }

        public void Run()
        {
            layouterCore.GenerateImage(wordsReadService)
                .OnFail(x => Console.WriteLine(x, Console.Error))
                .Then(x => imageSaver.Save(x))
                .Then(x => x.Dispose());
        }
    }
}