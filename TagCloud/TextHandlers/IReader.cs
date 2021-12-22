using System.Collections.Generic;

namespace TagCloud.TextHandlers;

public interface IReader
{
    public Result<IEnumerable<string>> Read(string filename);
}