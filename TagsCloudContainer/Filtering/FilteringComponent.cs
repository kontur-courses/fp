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
            foreach (var filter in filters)
            {
                words = filter.Filter(words).GetValueOrThrow();
            }

            return Result.Ok(new ReadOnlyCollection<string>(words));
        }

        public void AddFilter(IWordsFilter wordsFilter)
        {
            filters.Add(wordsFilter);
        }
    }
}