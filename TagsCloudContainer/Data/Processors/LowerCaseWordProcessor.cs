using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data.Processors
{
    public class LowerCaseWordProcessor : IWordProcessor
    {
        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower()).AsResult();
        }
    }
}