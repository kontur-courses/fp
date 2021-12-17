using ResultOf;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface IWordNormalizer : IService
{
    Result<string> Normalize(string word);
}