using System;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.GeneratorCore.CloudLayouters;
using TagCloudGenerator.GeneratorCore.CloudVocabularyPreprocessors;
using TagCloudGenerator.GeneratorCore.Extensions;
using TagCloudGenerator.ResultPattern;
using TagCloudGenerator.ResultPattern.DoHelper;

namespace TagCloudGenerator.GeneratorCore
{
    public class CloudGenerator
    {
        private readonly ICloudContextGenerator cloudContextGenerator;
        private readonly Func<TagCloudContext, CloudVocabularyPreprocessor> vocabularyPreprocessorConstructor;

        public CloudGenerator(ICloudContextGenerator cloudContextGenerator,
                              Func<TagCloudContext, CloudVocabularyPreprocessor> vocabularyPreprocessorConstructor)
        {
            this.cloudContextGenerator = cloudContextGenerator;
            this.vocabularyPreprocessorConstructor = vocabularyPreprocessorConstructor;
        }

        public Result<None> CreateTagCloudImage()
        {
            var tagCloudContext = cloudContextGenerator.GetTagCloudContext();

            var shuffledContentStrings = tagCloudContext
                .SelectValue(context => (preprocessor: vocabularyPreprocessorConstructor(context),
                                         cloudContent: context.TagCloudContent))
                .SelectValue(tuple => tuple.preprocessor.Process(tuple.cloudContent))
                .SelectValue(words => words.SequenceShuffle(new Random()))
                .SelectValue(words => words.Distinct())
                .SelectValue(words => words.ToArray());

            var layouter = tagCloudContext.SelectValue(context => context.CloudLayouter);
            var size = tagCloudContext.SelectValue(context => context.ImageSize);
            var imageName = tagCloudContext.SelectValue(context => context.ImageName);

            return tagCloudContext
                .Then(context => Do.Call<string[], ICloudLayouter, Size, Result<Bitmap>>(context.Cloud.CreateBitmap)
                          .With(shuffledContentStrings, layouter, size))
                .ActionOverValue(bitmap => Do.Call<string>(bitmap.Save).With(imageName))
                .ReplaceError(error => $"Bitmap creation error was handled:{Environment.NewLine}{error}");
        }
    }
}