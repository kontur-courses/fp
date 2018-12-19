using System.Collections.Generic;
using System.Linq;
using TagsCloudResult.Algorithms;
using TagsCloudResult.Loggers;
using TagsCloudResult.Settings;
using TagsCloudResult.SourceTextReaders;
using TagsCloudResult.TextPreprocessors;

namespace TagsCloudResult.DataProviders
{
    public class DataProvider : IDataProvider
    {
        private readonly ICloudSettings cloudSettings;
        private readonly ISourceTextReader textReader;
        private readonly IWordsPreprocessor wordsPreprocessor;
        private readonly IAlgorithm algorithm;
        private readonly ILogger logger;

        public DataProvider(ICloudSettings cloudSettings, ISourceTextReader textReader, IWordsPreprocessor wordsPreprocessor,
            IAlgorithm algorithm, ILogger logger)
        {
            this.cloudSettings = cloudSettings;
            this.textReader = textReader;
            this.wordsPreprocessor = wordsPreprocessor;
            this.algorithm = algorithm;
            this.logger = logger;
        }

        public Result<IReadOnlyCollection<Tag>> GetData()
        {
            return Result.Of(textReader.ReadText)
                .Then(lines => wordsPreprocessor.PreprocessWords(lines.Value))
                .OnFail(logger.Log)
                .Then(preprocessedWords => preprocessedWords
                    .OrderByDescending(e => e.Value))
                .Then(orderedWords => algorithm.GenerateTags(orderedWords
                    .Take(cloudSettings.WordsToDisplay).ToDictionary(e => e.Key, e => e.Value)).Value)
                .OnFail(logger.Log);
        }
    }
}