using System.Collections.Generic;

namespace TagsCloudResult.DataProviders
{
    public interface IDataProvider
    {
        Result<IReadOnlyCollection<Tag>> GetData();
    }
}