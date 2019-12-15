using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.WordConverters
{
    public class LowerCaseWordConverter : IWordConverter
    {
        public Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words)
        {
            return Result.Of(() => words.Select(word => word.ToLower()));
        }
    }
}