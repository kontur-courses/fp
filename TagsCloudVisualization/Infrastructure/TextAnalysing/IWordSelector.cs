using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.TextAnalysing
{
    public interface IWordSelector
    {
        Result<IEnumerable<string>> GetWords(string text);
    }
}