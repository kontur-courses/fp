using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.TextAnalysing
{
    public interface ITokenGenerator
    {
        Result<IEnumerable<Token>> GetTokens(string text, int maxTokenCount);
    }
}