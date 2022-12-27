using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public class EmptyLayouterAlgorithmProvider : ILayouterAlgorithmProvider
{
    public Result<ILayouterAlgorithm> Provide()
    {
        return Result.Failure<ILayouterAlgorithm>(nameof(EmptyLayouterAlgorithmProvider));
    }
}