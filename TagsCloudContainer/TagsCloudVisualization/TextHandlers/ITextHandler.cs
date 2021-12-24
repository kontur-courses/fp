using System.Collections.Generic;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.TextHandlers
{
    public interface ITextHandler
    {
        public Result<IEnumerable<string>> Handle(string filePath);
    }
}