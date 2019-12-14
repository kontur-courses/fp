using System.Collections.Generic;
using ErrorHandler;
using TagsCloudVisualization.Logic;

namespace TagsCloudVisualization.Services
{
    public interface IParser
    {
        Result<IEnumerable<WordToken>> ParseToTokens(string text);
    }
}