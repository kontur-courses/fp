using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public class FileReaderFactory : IFileReaderFactory
{
    private readonly IReadOnlyDictionary<string, IFileReader> fileReaders;

    public FileReaderFactory(IEnumerable<IFileReader> fileReaders)
    {
        this.fileReaders = CreateExtensionsDictionary(fileReaders);
    }

    public Result<IFileReader> Create(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        var extension = fileInfo.Extension;

        return fileReaders.ContainsKey(extension)
            ? Result.Ok(fileReaders[extension])
            : Result.Fail<IFileReader>("Unsupported input file extension");
    }

    private static Dictionary<string, IFileReader> CreateExtensionsDictionary(IEnumerable<IFileReader> fileReaders)
    {
        var dictionary = new Dictionary<string, IFileReader>();

        foreach (var fileReader in fileReaders)
        {
            foreach (var extension in fileReader.GetSupportedExtensions())
                dictionary[extension] = fileReader;
        }

        return dictionary;
    }
}