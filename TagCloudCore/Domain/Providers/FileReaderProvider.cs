using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCore.Domain.Providers;

public class FileReaderProvider : IFileReaderProvider
{
    private readonly Dictionary<string, IFileReader> _wordsFileReaders;
    private readonly IWordsPathSettingsProvider _pathSettingsProvider;

    public FileReaderProvider(
        IEnumerable<IFileReader> wordsFileReaders,
        IWordsPathSettingsProvider pathSettingsProvider)
    {
        _wordsFileReaders = wordsFileReaders.ToDictionary(reader => reader.SupportedExtension);
        _pathSettingsProvider = pathSettingsProvider;
    }

    public IEnumerable<string> SupportedExtensions => _wordsFileReaders.Keys;

    public Result<IFileReader> GetReader()
    {
        var wordsFileExtension = Path.GetExtension(_pathSettingsProvider.GetWordsPathSettings().WordsPath);
        return _wordsFileReaders.TryGetValue(wordsFileExtension, out var result)
            ? result.AsResult()
            : Result.Fail<IFileReader>($"No reader for extension: {wordsFileExtension}");
    }
}