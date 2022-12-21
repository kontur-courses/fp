using System.Collections.Generic;
using TagCloud.ResultMonade;

namespace TagCloud.TextParsing
{
    public interface ITextParser
    {
        Result<List<string>> GetWords(string text);
    }
}
