using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IValidator<in T>
{
    Result Validate(T value);
}