using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface ILayouterAlgorithmFactory
{
    Result<ILayouterAlgorithmProvider> Build(LayouterAlgorithmSettings layouterAlgorithmSettings);
}