namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public class FromFileStreamContext
{
    public readonly IFileEncoder FileEncoder;
    public readonly string Filename;

    public FromFileStreamContext(string filename, IFileEncoder fileEncoder)
    {
        Filename = filename;
        FileEncoder = fileEncoder;
    }
}