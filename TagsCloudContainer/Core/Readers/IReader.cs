using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Core.Readers
{
    interface IReader
    {
        Result<IEnumerable<string>> ReadWords(string path);
        bool CanRead(string path);
    }
}