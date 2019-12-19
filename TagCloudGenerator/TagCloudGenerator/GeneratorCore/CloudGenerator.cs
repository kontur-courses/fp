using System;
using System.Linq;
using TagCloudGenerator.GeneratorCore.CloudVocabularyPreprocessors;
using TagCloudGenerator.GeneratorCore.Extensions;
using TagCloudGenerator.ResultPattern;

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
            var cloudContextResult = cloudContextGenerator.GetTagCloudContext();

            if (!cloudContextResult.IsSuccess)
                return Result.Fail<None>(cloudContextResult.Error);

            var cloudContext = cloudContextResult.Value;
            var vocabularyPreprocessor = vocabularyPreprocessorConstructor(cloudContext);
            var processedVocabulary = vocabularyPreprocessor.Process(cloudContext.TagCloudContent).ToArray();

            var shuffledContentStrings = processedVocabulary
                .Take(1)
                .Concat(processedVocabulary.Skip(1).SequenceShuffle(new Random()))
                .Distinct()
                .ToArray();

            return cloudContext.Cloud.CreateBitmap(shuffledContentStrings,
                                                   cloudContext.CloudLayouter,
                                                   cloudContext.ImageSize)
                .Then(bitmap => bitmap.Save(cloudContext.ImageName))
                .ReplaceError(error => $"Bitmap creation error was handled:{Environment.NewLine}{error}");
        }
    }
}