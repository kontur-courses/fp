using System.Collections.Generic;
using ResultOf;
using TagCloud.Infrastructure;

namespace TagCloud.Visualization
{
    public interface ITagCloudElementsPreparer
    {
        Result<IEnumerable<TagCloudElement>> PrepareTagCloudElements(IEnumerable<Word> words);
        int CurrentWordIndex { get; }
    }
}
