using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Filtering
{
    public class FilteringComponent
    {
        private readonly List<IWordsFilter> filters;

        public FilteringComponent(IWordsFilter[] filters)
        {
            this.filters = filters.ToList();
        }

        public Result<ReadOnlyCollection<string>> FilterWords(ReadOnlyCollection<string> words)
        {
            var result = Result.Ok(words);
            foreach (var filter in filters)
            {
                result = result.Then(x => filter.Filter(x));
            }

            return result;
        }

        public void AddFilter(IWordsFilter wordsFilter)
        {
            filters.Add(wordsFilter);
        }
    }
}