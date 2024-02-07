using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public interface ITextReaderFactory
{
    public Result<ITextReader> GetTextReader();
}
