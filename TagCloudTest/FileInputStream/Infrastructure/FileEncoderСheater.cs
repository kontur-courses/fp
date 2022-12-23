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

    public string GetExpectedFileType()
    {
        return fileType;
    }

    public string GetText(string fileName)
    {
        return existFile
            ? text
            : throw new ArgumentException("File not found");
    }

    public bool IsCompatibleFileType(string fileName)
    {
        return fileName.EndsWith(fileType);
    }
}