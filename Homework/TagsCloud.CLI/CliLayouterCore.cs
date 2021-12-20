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
        private readonly IWordsProvider wordsProvider;

        public CliLayouterCore(
            ILayouterCore layouterCore,
            IImageSaver imageSaver,
            IWordsProvider wordsProvider)
        {
            this.layouterCore = layouterCore;
            this.imageSaver = imageSaver;
            this.wordsProvider = wordsProvider;
        }

        public void Run()
        {
            layouterCore.GenerateImage(wordsProvider)
                .OnFail(x => Console.WriteLine(x, Console.Error))
                .Then(x => imageSaver.Save(x))
                .Then(x => x.Dispose());
        }
    }
}