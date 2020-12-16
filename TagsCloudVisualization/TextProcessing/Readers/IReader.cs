namespace TagsCloudVisualization.TextProcessing.Readers
{
    public interface IReader
    {
        Result<string> ReadText(string path);
        bool CanReadFile(string path);
    }
}