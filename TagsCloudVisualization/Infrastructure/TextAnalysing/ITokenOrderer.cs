using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.TextAnalysing
{
    public interface ITokenOrderer
    {
        Result<IEnumerable<Token>> OrderTokens(IEnumerable<Token> tokens);
    }
}