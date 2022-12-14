using TagCloudCore.Infrastructure;

namespace TagCloudCore.Interfaces;

public interface IWordsInfoParser
{
    IEnumerable<WordInfo> GetWordsInfo();
}