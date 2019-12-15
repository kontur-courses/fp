using System.Collections.Generic;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.WordSizing
{
    public interface IWordSizer
    {
        Result<IEnumerable<SizedWord>> GetSizedWords(IEnumerable<string> words, int minSize, int step, int maxSize);
    }
}