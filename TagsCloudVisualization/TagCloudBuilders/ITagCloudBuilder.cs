using System.Collections.Generic;
using TagsCloudVisualization.Tags;
using TagsCloudVisualization.Words;

namespace TagsCloudVisualization.TagCloudBuilders
{
    public interface ITagCloudBuilder
    {
        Result<IReadOnlyList<Tag>> Build(IEnumerable<Word> wordsFrequency);
    }
}