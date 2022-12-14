using TagCloudCore.Infrastructure;

namespace TagCloudCore.Interfaces.Providers;

public interface IWordsPaintDataProvider
{
    IEnumerable<WordPaintData> GetWordsPaintData();
}