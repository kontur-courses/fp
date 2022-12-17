using TagCloudCore.Infrastructure;
using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces;

public interface IWordsInfoParser
{
    Result<IEnumerable<WordInfo>> GetWordsInfo();
}