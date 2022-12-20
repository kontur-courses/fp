namespace TagsCloudVisualization.Parsing;

public interface ITextParser
{
    Result<IEnumerable<string>> ParseWords(string text);
}