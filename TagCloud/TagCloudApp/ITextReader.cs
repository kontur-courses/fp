using System.Collections.Generic;
using Result;

namespace TagCloudApp
{
    internal interface ITextReader
    {
        string Extension { get; }

        Result<IEnumerable<string>> ReadWords(string path);
//        bool TryReadWords(string path, out IEnumerable<string> words);
    }
}
