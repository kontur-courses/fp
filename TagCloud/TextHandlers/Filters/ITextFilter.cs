using System.Collections.Generic;

namespace TagCloud.TextHandlers.Filters;

public interface ITextFilter
{
    Result<IEnumerable<string>> Filter(IEnumerable<string> wordsResult);
}