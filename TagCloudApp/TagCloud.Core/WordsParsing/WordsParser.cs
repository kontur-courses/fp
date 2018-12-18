using System.Collections.Generic;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;
using TagCloud.Core.WordsParsing.WordsProcessing;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Core.WordsParsing
{
    public class WordsParser
    {
        private readonly ITextParsingSettings settings;
        private readonly IWordsProcessor wordsProcessor;
        private readonly GeneralWordsReader generalWordsReader;

        public WordsParser(GeneralWordsReader generalWordsReader, IWordsProcessor wordsProcessor, ITextParsingSettings settings)
        {
            this.generalWordsReader = generalWordsReader;
            this.wordsProcessor = wordsProcessor;
            this.settings = settings;
        }

        public Result<IEnumerable<TagStat>> Parse(string pathToWords, string pathToBoringWords = null)
        {
            var wordsResult = generalWordsReader.ReadFrom(pathToWords)
                .AppendToErrorFromLeft($"Can't read words from \"{pathToWords}\":\n");
            if (!wordsResult.IsSuccess)
                return Result.Fail<IEnumerable<TagStat>>(wordsResult.Error);

            var boringWordsResult = string.IsNullOrEmpty(pathToBoringWords)
                ? Result.Ok(new HashSet<string>())
                : generalWordsReader.ReadFrom(pathToBoringWords)
                    .Then(readWords => new HashSet<string>(readWords))
                    .AppendToErrorFromLeft($"Can't read boring words from \"{pathToBoringWords}\":\n");

            return boringWordsResult.IsSuccess
                ? wordsProcessor.Process(wordsResult.Value, boringWordsResult.Value, settings.MaxUniqueWordsCount)
                : Result.Fail<IEnumerable<TagStat>>(boringWordsResult.Error);
        }
    }
}