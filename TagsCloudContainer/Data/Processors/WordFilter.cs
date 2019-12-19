using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data.Processors
{
    public class WordFilter : IWordProcessor
    {
        private readonly ISet<string> excluded;

        public WordFilter(IEnumerable<string> excluded)
        {
            this.excluded = new HashSet<string>(excluded);
        }

        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return words.Where(word => !excluded.Contains(word)).AsResult();
        }
    }
}