using ResultOfTask;

namespace TagsCloudContainer.Interfaces;

public interface IOptionsValidator
{
    Result<ICustomOptions> ValidateOptions(CustomOptions options);
}