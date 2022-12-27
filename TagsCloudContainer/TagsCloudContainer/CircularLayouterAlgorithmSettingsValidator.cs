using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class CircularLayouterAlgorithmSettingsValidator : IValidator<CircularLayouterAlgorithmSettings>
{
    public Result Validate(CircularLayouterAlgorithmSettings value)
    {
        return Result.Success();
    }
}