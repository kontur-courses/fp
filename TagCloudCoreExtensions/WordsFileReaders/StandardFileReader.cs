using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.WordsFileReaders;

public abstract class StandardFileReader : IFileReader
{
    private readonly IWordsPathSettingsProvider _pathSettingsProvider;

    protected StandardFileReader(IWordsPathSettingsProvider pathSettingsProvider)
    {
        _pathSettingsProvider = pathSettingsProvider;
    }

    public abstract string SupportedExtension { get; }

    public Result<string> ReadFile()
    {
        var path = _pathSettingsProvider.GetWordsPathSettings().WordsPath;
        if (!File.Exists(path))
            return Result.Fail<string>($"File not found. Wrong path: {path}");
        var ext = Path.GetExtension(path);
        return ext == SupportedExtension
            ? Result.Of(() => InternalReadFile(path))
            : Result.Fail<string>($"Wrong file extension: {ext}");
    }

    protected abstract string InternalReadFile(string path);
}