using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITokenOrderer
    {
        Result<IEnumerable<Token>> OrderTokens(IEnumerable<Token> tokens);
    }
}