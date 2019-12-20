using System.IO;
using ResultOf;

namespace TagCloud.TextReading
{
    public interface ITextReaderSelector
    {
        Result<ITextReader> GetTextReader(FileInfo file);
    }
}
