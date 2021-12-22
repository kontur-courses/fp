using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.TextAnalysing
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IWordSelector wordSelector;
        private readonly ITokenWeigher tokenWeigher;
        private readonly ITokenOrderer tokenOrderer;

        public TokenGenerator(IWordSelector wordSelector, ITokenWeigher tokenWeigher, ITokenOrderer tokenOrderer)
        {
            this.wordSelector = wordSelector;
            this.tokenWeigher = tokenWeigher;
            this.tokenOrderer = tokenOrderer;
        }

        public Result<IEnumerable<Token>> GetTokens(string text, int maxCount)
        {
            return wordSelector.GetWords(text)
                .Then(words => tokenWeigher.Evaluate(words, maxCount))
                .Then(tokens => tokenOrderer.OrderTokens(tokens));
        }
    }
}