using System.Collections.Generic;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;
using TagCloud.Core.WordsParsing.WordsProcessing;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Core.WordsParsing
{
    public class WordsParser
    {
        private readonly IWordsReader wordsReader;
        private readonly IWordsProcessor wordsProcessor;
        private readonly ITextParsingSettings settings;

        public WordsParser(IWordsReader wordsReader, IWordsProcessor wordsProcessor, ITextParsingSettings settings)
        {
            this.wordsReader = wordsReader;
            this.wordsProcessor = wordsProcessor;
            this.settings = settings;
        }

        public Result<IEnumerable<TagStat>> Parse(string pathToWords, string pathToBoringWords = null)
        {
            var wordsResult = wordsReader.ReadFrom(pathToWords);
            if (!wordsResult.IsSuccess) return Result.Fail<IEnumerable<TagStat>>(wordsResult.Error);

            var boringWordsResult = pathToBoringWords == null
                ? Result.Ok(new HashSet<string>())
                : wordsReader.ReadFrom(pathToBoringWords)
                    .Then(readWords => new HashSet<string>(readWords));
            if (!boringWordsResult.IsSuccess) Result.Fail<IEnumerable<TagStat>>(boringWordsResult.Error);

            return wordsProcessor.Process(wordsResult.Value, boringWordsResult.Value, settings.MaxUniqueWordsCount);
        }
    }
}