using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public class SourceSettings
{
    public string Path { get; set; } = @"";

    public Result<SourceSettings> Validate()
    {
        return ValidatePathNotNullOrEmpty(this)
            .Then(ValidatePathExists);
    }

    private static Result<SourceSettings> ValidatePathNotNullOrEmpty(SourceSettings settings)
    {
        return Result.Validate(settings, x => !string.IsNullOrEmpty(x.Path),
            Resources.SourceSettings_ValidatePathNotNullOrEmpty_Fail);
    }

    private static Result<SourceSettings> ValidatePathExists(SourceSettings settings)
    {
        return Result.Validate(settings, x => File.Exists(x.Path),
            string.Format(Resources.SourceSettings_ValidatePathExists_Fail, System.IO.Path.GetFullPath(settings.Path)));
    }
}
