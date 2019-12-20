using System.Collections.Generic;
using System.IO;
using ResultOf;

namespace TagCloud.TextReading
{
    public interface ITextReader
    { 
        Result<IEnumerable<string>> ReadWords(FileInfo file);
        string Extension { get; }
    }
}
