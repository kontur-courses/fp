using System.Collections.Generic;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.DataReader
{
    public interface IDataReader
    {
        Result<IEnumerable<string>> Read(string filename);
    }
}