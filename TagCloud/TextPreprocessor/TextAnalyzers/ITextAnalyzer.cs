using System.Collections.Generic;
using ResultLogic;
using TagCloud.TextPreprocessor.Core;

namespace TagCloud.TextPreprocessor.TextAnalyzers
{
    public interface ITextAnalyzer
    {
        Result<IEnumerable<TagInfo>> GetTagInfo(IEnumerable<Tag> tags);
    }
}