using TagCloud.FileConverter;
using TagCloud.ResultImplementation;

namespace TagCloud.FileReader.Implementation;

public class DocxReader : IFileReader
{
    private readonly IFileConverter converter;

    public DocxReader(IFileConverter converter)
    {
        this.converter = converter;
    }

    public Result<string[]> Read(string path)
    {
        var newPath = converter.Convert(path);
        return newPath.IsSuccess
            ? File.ReadAllLines(newPath.Value).Skip(1).ToArray()
            : Result.Fail<string[]>(newPath.Error);
    }
}