namespace TagsCloudVisualization.Parsing;

public class SingleColumnTextParser : ITextParser
{
    public Result<IEnumerable<string>> ParseWords(string text)
    {
        return text.Split(Environment.NewLine);
    }
}