using System.Drawing;
using System.Drawing.Text;
using System.Linq;
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

        public override Result<ITagsGenerator> Create()
        {
            if (wordsConfig.Font.OriginalFontName != null 
                && wordsConfig.Font.Name != wordsConfig.Font.OriginalFontName)
                return Result.Fail<ITagsGenerator>($"This font {wordsConfig.Font.OriginalFontName} not supported");
            return Result.Of(() => services[wordsConfig.TagGeneratorName](),
                                $"This tags generator {wordsConfig.TagGeneratorName ?? "null"} not supported");
        }
    }
}
