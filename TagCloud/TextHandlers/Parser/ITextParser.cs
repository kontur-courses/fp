using System.Collections.Generic;

namespace TagCloud.TextHandlers.Parser
{
    public interface ITextParser
    {
        Result<IEnumerable<string>> GetWords(string path);
    }
}