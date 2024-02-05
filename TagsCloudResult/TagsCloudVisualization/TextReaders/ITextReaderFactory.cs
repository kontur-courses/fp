using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public interface ITextReaderFactory
{
    public Result<TextReader> GetTextReader();
}
