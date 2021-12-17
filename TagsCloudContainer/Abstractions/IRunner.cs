using ResultOf;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface IRunner : IService
{
    Result Run(params string[] args);
}
