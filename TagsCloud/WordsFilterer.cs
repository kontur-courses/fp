using System.Collections.Immutable;
using TagsCloud.WordsFiltering;

namespace TagsCloud
{
    public class WordsFilterer
    {
        private readonly IFilter[] filters;

        public WordsFilterer(IFilter[] filters)
        {
            this.filters = filters;
        }

        public Result<ImmutableList<string>> FilterWords(ImmutableList<string> words)
        {
            var res = Result.Ok(words);            
            foreach (var filter in filters)
                res = res.Then(items => filter.FilterWords(items));
            return res;
        }
    }
}
