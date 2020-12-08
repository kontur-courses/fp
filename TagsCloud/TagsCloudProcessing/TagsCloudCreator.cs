using TagsCloud.Factory;
using TagsCloud.ImageProcessing.ImageBuilders;
using TagsCloud.ImageProcessing.SaverImage.ImageSavers;
using TagsCloud.ResultOf;
using TagsCloud.TagsCloudProcessing.TagsGenerators;
using TagsCloud.TextProcessing;

namespace TagsCloud.TagsCloudProcessing
{
    public class TagsCloudCreator
    {
        private readonly TextProcessor textProcessor;
        private readonly IImageBuilder imageBuilder;
        private readonly IServiceFactory<IImageSaver> imageSaverFactory;
        private readonly IServiceFactory<ITagsGenerator> tagsGeneratorFactory;

        public TagsCloudCreator(TextProcessor textProcessor,
             IImageBuilder imageBuilder, IServiceFactory<IImageSaver> imageSaverFactory,
             IServiceFactory<ITagsGenerator> tagsGeneratorFactory)
        {
            this.tagsGeneratorFactory = tagsGeneratorFactory;
            this.textProcessor = textProcessor;
            this.imageSaverFactory = imageSaverFactory;
            this.imageBuilder = imageBuilder;
        }

        public Result<None> CreateCloud(string textPath, string imageSavePath)
        {
            var wordsFromFile = textProcessor.ReadFromFile(textPath);
            var tags = tagsGeneratorFactory.Create()
                .Then(generator => wordsFromFile.Then(words => generator.CreateTags(words)));

            var image = tags.Then(tags => imageBuilder.BuildImage(tags));
            return imageSaverFactory.Create()
                .Then(saver => image.Then(img => saver.SaveImage(img, imageSavePath)));
        }
    }
}
