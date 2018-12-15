using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudResult.Algorithms;
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

        public DataProvider(ICloudSettings cloudSettings, ISourceTextReader textReader, IWordsPreprocessor wordsPreprocessor, IAlgorithm algorithm)
        {
            this.cloudSettings = cloudSettings;
            this.textReader = textReader;
            this.wordsPreprocessor = wordsPreprocessor;
            this.algorithm = algorithm;
        }

        public Result<IReadOnlyDictionary<string, (Rectangle, int)>> GetData()
        {
            return Result.Of(textReader.ReadText)
                .Then(lines => wordsPreprocessor.PreprocessWords(lines.Value))
                .Then(preprocessedWords => algorithm.GenerateRectanglesSet(preprocessedWords
                    .OrderByDescending(e => e.Value)
                    .Take(cloudSettings.WordsToDisplay).ToDictionary(e => e.Key, e => e.Value)).Value);
        }
    }
}