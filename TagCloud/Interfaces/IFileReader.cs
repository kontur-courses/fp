using System.Collections.Generic;

namespace TagCloud.Interfaces
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> Read(string path);
    }
}