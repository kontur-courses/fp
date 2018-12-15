using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.DataProviders
{
    public interface IDataProvider
    {
        Result<IReadOnlyDictionary<string, (Rectangle, int)>> GetData();
    }
}