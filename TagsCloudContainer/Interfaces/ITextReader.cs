using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Interfaces
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> Read(string path);
    }
}