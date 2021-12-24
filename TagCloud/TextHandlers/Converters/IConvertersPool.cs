using System.Collections.Generic;

namespace TagCloud.TextHandlers.Converters;

public interface IConvertersPool
{
    Result<IEnumerable<string>> Convert(IEnumerable<string> words);
}