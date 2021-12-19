using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITokenOrderer
    {
        IEnumerable<Token> OrderTokens(IEnumerable<Token> tokens);
    }
}