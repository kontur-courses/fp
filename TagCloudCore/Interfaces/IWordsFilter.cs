using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces;

public interface IWordsFilter
{
    public Result<IEnumerable<string>> FilterWords(IEnumerable<string> sourceWords);
}