using TagCloudCore.Infrastructure;
using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces.Providers;

public interface IWordsPaintDataProvider
{
    Result<IEnumerable<WordPaintData>> GetWordsPaintData();
}