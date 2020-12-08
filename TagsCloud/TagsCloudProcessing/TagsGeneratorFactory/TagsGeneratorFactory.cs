using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TagsCloudProcessing.TagsGenerators;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TagsCloudProcessing.TagsGeneratorFactory
{
    public class TagsGeneratorFactory : ServiceFactory<ITagsGenerator>
    {
        private readonly WordConfig wordsConfig;

        public TagsGeneratorFactory(WordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<ITagsGenerator> Create() =>
            Result.Of(() => services[wordsConfig.TagGeneratorName](),
                $"This tags generator {wordsConfig.TagGeneratorName ?? "null"} not supported");
    }
}
