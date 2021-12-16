using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.Preprocessing
{
    public class LowerCaseWordsPreprocessor : IWordsPreprocessor
    {
        public Result<IEnumerable<string>> Preprocess(IEnumerable<string> words) =>
            words.Select(word => word.ToLower()).AsResult();
    }
}