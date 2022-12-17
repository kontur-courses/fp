using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces;

public interface IWordsNormalizer
{
    public Result<IEnumerable<string>> GetWordsOriginalForm(string sourceText);
}