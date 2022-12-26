using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Forms.Interfaces;

namespace TagCloudContainer.Forms.Validators;

public class TagCloudContainerConfigValidator : IConfigValidator<ITagCloudContainerConfig>
{
    public Result<ITagCloudContainerConfig> Validate(ITagCloudContainerConfig tagCloudContainerConfig)
    {
        if (!File.Exists(tagCloudContainerConfig.WordsFilePath))
            return Result.Fail<ITagCloudContainerConfig>("Words file does not exist");
        if (!File.Exists(tagCloudContainerConfig.ExcludeWordsFilePath))
            return Result.Fail<ITagCloudContainerConfig>("Exclude words file does not exist");
        
        return Result.Ok(tagCloudContainerConfig);       
    }
}