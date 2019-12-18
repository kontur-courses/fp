using System.Collections.Generic;
using System.Linq;
using TagCloudResult;

namespace TagsCloudTextProcessing.Shufflers
{
    public class DescendingCountShuffler : ITokenShuffler
    {
        public Result<List<Token>> Shuffle(IEnumerable<Token> tokens)
        {
            return tokens.OrderByDescending(token => token.Count).ToList();
        }
    }
}