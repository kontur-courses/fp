namespace TagsCloudVisualization
{
    public interface IFileReader
    {
        Result<string> Read(string fileName);
    }
}
