using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Cli;

public class CliGraphicsProviderSettingsValidator : IValidator<CliGraphicsProviderSettings>
{
    public Result Validate(CliGraphicsProviderSettings value)
    {
        return Result.SuccessIf(() => value.Width > 0, "Width of images is less than or equal zero.")
            .CombineCheck(() => value.Height > 0, "Height of images is less than or equal zero.")
            .CombineCheck(() => Directory.Exists(value.BasePath), "Base path is not existed directory")
            .CombineCheck(() => FileExtensions.CheckWritingAccessToDirectory(value.BasePath),
                "Base path is not writable for current user");
    }
}