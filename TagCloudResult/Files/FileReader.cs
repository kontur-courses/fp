using ResultOfTask;

namespace TagCloudResult.Files;

public class FileReader
{
    private readonly IEnumerable<IFileReader> _fileReaders;

    public FileReader(IEnumerable<IFileReader> fileReaders)
    {
        _fileReaders = fileReaders;
    }

    public Result<string> ReadAll(string filename)
    {
        var extension = Path.GetExtension(filename);
        var fileReader = _fileReaders.FirstOrDefault(file => file.Extension == extension);
        return fileReader is null
            ? Result.Fail<string>($"This extension {extension} is not supported!")
            : Result.Of(() => fileReader.ReadAll(filename))
                .RefineError($"Cannot read the file '{filename}'");
    }
}