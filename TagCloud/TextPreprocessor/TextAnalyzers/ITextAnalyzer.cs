using System.Collections.Generic;
using TagCloud.TextPreprocessor.Core;
using TagsCloud;

namespace TagCloud.TextPreprocessor.TextAnalyzers
{
    public interface ITextAnalyzer
    {
        Result<IEnumerable<TagInfo>> GetTagInfo(IEnumerable<Tag> tags);
    }
}