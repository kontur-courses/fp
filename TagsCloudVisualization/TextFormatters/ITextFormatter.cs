using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.TextFormatters
{
    public interface ITextFormatter
    {
        public Result<List<Word>> Format(string text);
    }
}