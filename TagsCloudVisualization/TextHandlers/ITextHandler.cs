using Results;

namespace TagsCloudVisualization.TextHandlers;

public interface ITextHandler
{
    Result<Dictionary<string, int>> HandleText();
}
