namespace TagsCloudPainter.FileReader;

public interface IFormatFileReader<TFormat>
{
    public Result<TFormat> ReadFile(string path);
}