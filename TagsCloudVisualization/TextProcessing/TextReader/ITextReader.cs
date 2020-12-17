namespace TagsCloudVisualization.TextProcessing.TextReader
{
    public interface ITextReader
    {
        Result<string> ReadAllText(string path);
    }
}