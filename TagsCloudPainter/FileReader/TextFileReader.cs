using ResultLibrary;

namespace TagsCloudPainter.FileReader;

public class TextFileReader : IFormatFileReader<string>
{
    private readonly IEnumerable<IFileReader> fileReaders;

    public TextFileReader(IEnumerable<IFileReader> fileReaders)
    {
        this.fileReaders = fileReaders ?? throw new ArgumentNullException(nameof(fileReaders));
    }

    public Result<string> ReadFile(string path)
    {
        if (!File.Exists(path))
            return Result.Fail<string>($"{path} file was not found");

        var fileExtension = Result.Of(() => Path.GetExtension(path));
        var fileReader = fileExtension.Then(extension =>
            Result.Of(() => fileReaders.First(fileReader => fileReader.SupportedExtensions.Contains(extension)), 
            $"Incorrect file extension {fileExtension}. Supported file extensions: txt, doc, docx"));

        return fileReader.Then(reader => reader.ReadFile(path));
    }
}