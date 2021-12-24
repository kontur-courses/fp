using System.Collections.Generic;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.TextPreparers
{
    public interface ITextPreparer
    {
        public Result<IEnumerable<string>> PrepareText(IEnumerable<string> text);
    }
}