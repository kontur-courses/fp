using System.Collections.Generic;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.FileReader
{
    public interface IFileReader
    {
        OperationResult<IEnumerable<string>> Read();
    }
}