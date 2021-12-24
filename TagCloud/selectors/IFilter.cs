using System.Collections.Generic;

namespace TagCloud.selectors
{
    public interface IFilter<T>
    {
        Result<IEnumerable<T>> Filter(IEnumerable<string> source);
    }
}