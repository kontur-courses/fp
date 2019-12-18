using System;
using System.Collections.Generic;
using System.Linq;
using TagCloudResult;

namespace TagsCloudTextProcessing.Shufflers
{
    public class RandomShuffler : ITokenShuffler

    {
        private readonly int randomSeed;

        public RandomShuffler(int randomSeed)
        {
            this.randomSeed = randomSeed;
        }

        public Result<List<Token>> Shuffle(IEnumerable<Token> tokens)
        {
            var random = new Random(randomSeed);
            var tokensList = tokens.ToList();
            var resultList = new List<Token>();
            while (tokensList.Count > 0)
            {
                var randomIndex = random.Next(0, tokensList.Count);
                resultList.Add(tokensList[randomIndex]);
                tokensList.RemoveAt(randomIndex);
            }

            return Result.Ok(resultList);
        }
    }
}