using System;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.ImagesSavers;
using TagsCloud.Visualization.TagsCloudVisualizers;
using TagsCloud.Visualization.TextProviders;

namespace TagsCloud.Words
{
    public class CliTagsCloudVisualizer
    {
        private readonly IImageSaver imageSaver;
        private readonly ITagsCloudVisualizer tagsCloudVisualizer;
        private readonly ITextProvider textProvider;

        public CliTagsCloudVisualizer(
            ITagsCloudVisualizer tagsCloudVisualizer,
            IImageSaver imageSaver,
            ITextProvider textProvider)
        {
            this.tagsCloudVisualizer = tagsCloudVisualizer;
            this.imageSaver = imageSaver;
            this.textProvider = textProvider;
        }

        public void Run()
        {
            tagsCloudVisualizer.GenerateImage(textProvider)
                .OnFail(x => Console.WriteLine(x, Console.Error))
                .Then(x => imageSaver.Save(x))
                .Then(x => x.Dispose());
        }
    }
}