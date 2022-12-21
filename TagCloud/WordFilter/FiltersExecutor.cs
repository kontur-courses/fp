using System.Collections.Generic;
using System.Linq;
using TagCloud.ResultMonade;

namespace TagCloud.WordFilter
{
    public class FiltersExecutor : IFiltersExecutor
    {
        private readonly List<IWordFilter> Filters = new List<IWordFilter>();

        public FiltersExecutor(IWordFilter[] filters)
        {
            foreach (var filter in filters)
                Filters.Add(filter);
        }

        public Result<IEnumerable<string>> Filter(IEnumerable<string> words)
        {
            return words.AsResult()
                    .Then(ws => ws.Where(word => Filters.All(filter => filter.IsPermitted(word).Value)));
        }
    }
}
 