using TagCloud;
using TagCloud.App.WordPreprocessorDriver.InputStream;

namespace TagCloudTest.FileInputStream.Infrastructure;

public class FileEncoderСheater : IFileEncoder
{
    private readonly bool existFile;
    private readonly string fileType;
    private readonly string text;

    public FileEncoderСheater(string text, bool existFile, string fileType)
    {
        this.text = text;
        this.existFile = existFile;
        this.fileType = fileType;
    }

    public Result<string> GetText(string fileName)
    {
        return existFile
            ? Result.Ok(text)
            : Result.Fail<string>("File not found");
    }

    public Result<None> IsSuitableFileType(string fileName)
    {
        return fileName.EndsWith(fileType)
            ? Result.Ok()
            : Result.Fail<None>("Not suitable file type");
    }

    public string GetExpectedFileType()
    {
        return fileType;
    }
}