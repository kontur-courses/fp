using System.Collections.Generic;
using TagCloud.ResultMonade;

namespace TagCloud.WordConverter
{
    public interface IConvertersExecutor
    {
        Result<List<string>> Convert(IEnumerable<string> words);
    }
}
