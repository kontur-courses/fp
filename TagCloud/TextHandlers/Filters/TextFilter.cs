using System;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud.TextHandlers.Filters;

internal class TextFilter : ITextFilter
{
    private readonly List<Predicate<string>> filters = new();

    public TextFilter()
    {
    }

    public TextFilter(params IFilter[] filters)
    {
        foreach (var filter in filters)
            this.filters.Add(filter.IsCorrectWord);
    }

    public Result<IEnumerable<string>> Filter(IEnumerable<string> words)
    {
        return words
            .AsResult()
            .Then(w => w.Where(word => filters.All(f => f(word))));
    }
}