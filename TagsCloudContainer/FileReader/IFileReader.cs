using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.FileReader
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> Read();
    }
}