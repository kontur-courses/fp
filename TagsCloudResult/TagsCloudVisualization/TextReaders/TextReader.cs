using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.TextReaders;

public abstract class TextReader
{
    protected readonly SourceSettings Settings;

    protected TextReader(SourceSettings settings)
    {
        Settings = settings;
    }

    public Result<string> GetText()
    {
        return ValidatePathNotNullOrEmpty(Settings.Path)
            .Then(ValidatePathExists)
            .Then(ReadText);
    }

    private static Result<string> ValidatePathNotNullOrEmpty(string path)
    {
        return Result.Validate(path, path => !string.IsNullOrEmpty(path),
            "В качестве пути к файлу источника была предоставлена пустая строка. Пожалуйста, укажите верный путь к файлу.");
    }

    private static Result<string> ValidatePathExists(string path)
    {
        return Result.Validate(path, File.Exists,
            $@"Указанного файла источника не существует: {Path.GetFullPath(path)}");
    }

    protected abstract Result<string> ReadText(string path);
}