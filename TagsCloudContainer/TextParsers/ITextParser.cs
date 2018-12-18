using System.Collections.Generic;

namespace TagsCloudContainer.TextParsers
{
    public interface ITextParser
    {
        Result<List<WordFrequency>> Parse(Result<string> text);
    }
}