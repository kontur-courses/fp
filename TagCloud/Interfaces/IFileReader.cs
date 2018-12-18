using System.Collections.Generic;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> Read(string path);
    }
}