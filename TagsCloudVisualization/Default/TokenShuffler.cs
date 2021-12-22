using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.TextAnalysing;

namespace TagsCloudVisualization.Default
{
    public class TokenShuffler : ITokenOrderer
    {
        public Result<IEnumerable<Token>> OrderTokens(IEnumerable<Token> tokens)
        {
            var rnd = new Random();
            return Result.Ok<IEnumerable<Token>>(tokens.OrderBy(x => rnd.Next()));
        }
    }
}