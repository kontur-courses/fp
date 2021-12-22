using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.TextAnalysing
{
    public interface ITokenWeigher
    {
        Result<Token[]> Evaluate(IEnumerable<string> words, int maxTokenCount);
    }
}