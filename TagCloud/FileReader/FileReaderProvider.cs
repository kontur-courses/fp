using ResultOf;

namespace TagCloud.FileReader;

public class FileReaderProvider : IFileReaderProvider
{
    private Dictionary<string, IFileReader> readers;

    public FileReaderProvider(IEnumerable<IFileReader> readers)
    {
        this.readers = ArrangeReaders(readers);
    }

    public Result<IFileReader> CreateReader(string inputPath)
    {
        var extension = inputPath.Split(".").Last();
        return readers.ContainsKey(extension)
            ? Result.Ok(readers[extension])
            : Result.Fail<IFileReader>($"Reading of file {inputPath} with extension {extension} is not supported");
    }

    private Dictionary<string, IFileReader> ArrangeReaders(IEnumerable<IFileReader> readers)
    {
        var readersDictionary = new Dictionary<string, IFileReader>();
        foreach (var reader in readers)
        {
            foreach (var extension in reader.GetAvailableExtensions())
            {
                readersDictionary[extension] = reader;
            }
        }

        return readersDictionary;
    }
}