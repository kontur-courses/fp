using System.Collections.Generic;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.Parsers
{
    public interface IParser
    {
        public Result<IEnumerable<string>> ParseWords(string filePath);
    }
}