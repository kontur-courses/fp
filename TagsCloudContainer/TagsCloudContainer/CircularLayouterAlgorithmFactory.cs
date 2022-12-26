using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class CircularLayouterAlgorithmFactory : ILayouterAlgorithmFactory
{
    public Result<ILayouterAlgorithmProvider> Build(LayouterAlgorithmSettings layouterAlgorithmSettings)
    {
        return layouterAlgorithmSettings.SuccessIfCast<LayouterAlgorithmSettings, CircularLayouterAlgorithmSettings>()
            .Bind(settings => Result.Success(new CircularLayouterAlgorithmProvider(settings)))
            .Bind(provider => Result.Success((ILayouterAlgorithmProvider)provider));
    }

    private sealed class CircularLayouterAlgorithmProvider : ILayouterAlgorithmProvider
    {
        private readonly CircularLayouterAlgorithmSettings circularLayouterAlgorithmSettings;

        public CircularLayouterAlgorithmProvider(CircularLayouterAlgorithmSettings circularLayouterAlgorithmSettings)
        {
            this.circularLayouterAlgorithmSettings = circularLayouterAlgorithmSettings;
        }

        public Result<ILayouterAlgorithm> Provide()
        {
            return new CircularLayouterAlgorithm(circularLayouterAlgorithmSettings);
        }
    }
}