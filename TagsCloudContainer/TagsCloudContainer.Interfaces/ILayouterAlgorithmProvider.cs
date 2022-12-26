using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface ILayouterAlgorithmProvider
{
    Result<ILayouterAlgorithm> Provide();
}