using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ITokenWeigher
    {
        Token[] Evaluate(IEnumerable<string> words, int maxTokenCount);
    }
}