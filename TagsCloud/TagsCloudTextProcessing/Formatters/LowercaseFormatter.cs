using System.Collections.Generic;
using System.Linq;
using TagCloudResult;

namespace TagsCloudTextProcessing.Formatters
{
    public class LowercaseFormatter : IWordsFormatter
    {
        public Result<IEnumerable<string>> Format(IEnumerable<string> wordsInput)
        {
            return Result.Ok(wordsInput.Select(w => w.ToLower()));
        }
    }
}