using System;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.ImagesSavers;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Words
{
    public class CliTagsCloudVisualizer
    {
        private readonly IImageSaver imageSaver;
        private readonly ITagsCloudVisualizer tagsCloudVisualizer;
        private readonly IWordsProvider wordsProvider;

        public CliTagsCloudVisualizer(
            ITagsCloudVisualizer tagsCloudVisualizer,
            IImageSaver imageSaver,
            IWordsProvider wordsProvider)
        {
            this.tagsCloudVisualizer = tagsCloudVisualizer;
            this.imageSaver = imageSaver;
            this.wordsProvider = wordsProvider;
        }

        public void Run()
        {
            tagsCloudVisualizer.GenerateImage(wordsProvider)
                .OnFail(x => Console.WriteLine(x, Console.Error))
                .Then(x => imageSaver.Save(x))
                .Then(x => x.Dispose());
        }
    }
}