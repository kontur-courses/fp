namespace TagCloudGenerator.TextProcessors
{
    public interface ITextProcessor
    {
        Result<IEnumerable<string>> ProcessText(IEnumerable<string> text);
    }
}