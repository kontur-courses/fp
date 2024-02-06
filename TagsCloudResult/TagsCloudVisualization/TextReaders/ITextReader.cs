using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public interface ITextReader
{
    public Result<string> GetText();
}
