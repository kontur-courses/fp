using System.Collections.Generic;

namespace TagsCloudResult.SourceTextReaders
{
    public interface ISourceTextReader
    {
        Result<IEnumerable<string>> ReadText();
    }
}