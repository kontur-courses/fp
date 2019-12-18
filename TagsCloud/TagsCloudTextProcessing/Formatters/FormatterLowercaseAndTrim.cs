using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.Formatters
{
    public class FormatterLowercaseAndTrim : IWordsFormatter
    {
        public Result<IEnumerable<string>> Format(IEnumerable<string> wordsInput)
        {
            return new TrimFormatter().Format(wordsInput)
                .Then(w => new LowercaseFormatter().Format(w));
        }
    }
}