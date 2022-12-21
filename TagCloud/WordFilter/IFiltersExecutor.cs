using System.Collections.Generic;
using TagCloud.ResultMonade;

namespace TagCloud.WordFilter
{
    public interface IFiltersExecutor
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> words);
    }
}
