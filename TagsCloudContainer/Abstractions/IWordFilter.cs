using ResultOf;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface IWordFilter : IService
{
    Result<bool> IsValid(string word);
}