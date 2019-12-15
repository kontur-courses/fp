using System.Collections.Generic;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.WordConverters
{
    public interface IWordConverter
    {
        Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words);
    }
}