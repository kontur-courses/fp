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
            "В качестве пути к файлу источника была предоставлена пустая строка. Пожалуйста, укажите верный путь к файлу.");
    }

    private static Result<SourceSettings> ValidatePathExists(SourceSettings settings)
    {
        return Result.Validate(settings, x => File.Exists(x.Path),
            $@"Файла источника по указанному пути не существует: {System.IO.Path.GetFullPath(settings.Path)}.");
    }
}
