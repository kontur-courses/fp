using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.TextAnalysing;

namespace TagsCloudVisualization.Default
{
    public class TokenSortedOrder : ITokenOrderer
    {
        public Result<IEnumerable<Token>> OrderTokens(IEnumerable<Token> tokens)
        {
            return Result.Ok<IEnumerable<Token>>(tokens.OrderByDescending(t => t.Weight)
                .ThenByDescending(t => t.Value.Length));
        }
    }
}