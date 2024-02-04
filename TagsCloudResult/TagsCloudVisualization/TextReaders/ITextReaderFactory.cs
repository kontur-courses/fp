using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.TextReaders;

public interface ITextReaderFactory
{
    public Result<TextReader> GetTextReader();
}