using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.MyStem;

namespace TagsCloudContainer.WordProcessing.Converting
{
    public class ToInitialFormWordConverter : IWordConverter
    {
        private readonly MyStemExecutor myStemExecutor;
        private readonly MyStemResultParser myStemResultParser;

        public ToInitialFormWordConverter(MyStemExecutor myStemExecutor, MyStemResultParser myStemResultParser)
        {
            this.myStemExecutor = myStemExecutor;
            this.myStemResultParser = myStemResultParser;
        }

        public Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words)
        {
            return Result.Of(() => myStemExecutor.GetMyStemResultForWords(words, "-ni"))
                .RefineError("Failed to execute MyStem")
                .Then(myStemResult => myStemResultParser.GetInitialFormsByResultOfNiCommand(myStemResult, words)
                    .Select(p => p.Item2))
                .RefineError("Failed to parse MyStem output");
        }
    }
}