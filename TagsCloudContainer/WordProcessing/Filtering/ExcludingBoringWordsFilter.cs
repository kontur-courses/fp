using ResultOf;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.MyStem;

namespace TagsCloudContainer.WordProcessing.Filtering
{
    public class ExcludingBoringWordsFilter : IWordFilter
    {
        private readonly MyStemExecutor myStemExecutor;
        private readonly MyStemResultParser myStemResultParser;

        public ExcludingBoringWordsFilter(MyStemExecutor myStemExecutor, MyStemResultParser myStemResultParser)
        {
            this.myStemExecutor = myStemExecutor;
            this.myStemResultParser = myStemResultParser;
        }

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return Result.Of(() => myStemExecutor.GetMyStemResultForWords(words, "-ni"))
                .RefineError("Failed to execute MyStem")
                .Then(myStemResult =>
                    myStemResultParser.GetPartsOfSpeechByResultOfNiCommand(myStemResult, words).ToList())
                .RefineError("Failed to parse MyStem result")
                .Then(FilterWordsWithPartsOfSpeech);
        }

        private IEnumerable<string> FilterWordsWithPartsOfSpeech(
            IEnumerable<(string, string)> wordsWithPartsOfSpeech)
        {
            return wordsWithPartsOfSpeech
                .Where(p => !IsPartOfSpeechBoring(p.Item2))
                .Select(p => p.Item1);
        }

        private bool IsPartOfSpeechBoring(string myStemPartOfSpeech)
        {
            return myStemPartOfSpeech == "PR" || myStemPartOfSpeech.EndsWith("PRO") || myStemPartOfSpeech == "CONJ" ||
                   myStemPartOfSpeech == "PART";
        }
    }
}