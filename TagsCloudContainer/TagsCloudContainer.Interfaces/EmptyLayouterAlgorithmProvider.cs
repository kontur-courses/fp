using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public class EmptyLayouterAlgorithmProvider : ILayouterAlgorithmProvider
{
    public Result<ILayouterAlgorithm> Provide() =>
        Result.Failure<ILayouterAlgorithm>(nameof(EmptyLayouterAlgorithmProvider));
}