using System.Collections.Generic;
using System.Linq;
using TagCloudResult;

namespace TagsCloudTextProcessing.Shufflers
{
    public class AscendingCountShuffler: ITokenShuffler
    {
        public Result<List<Token>> Shuffle(IEnumerable<Token> tokens)
        {
            return Result.Ok(tokens.OrderBy(token => token.Count).ToList());
        }
    }
}