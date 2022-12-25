namespace TagsCloudVisualization.TextProviders;

public interface ITextProvider
{
    Result<IEnumerable<string>> GetText();
}