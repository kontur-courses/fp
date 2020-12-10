using System.Collections.Generic;
using TagsCloud.TextProcessing.Tags;

namespace TagsCloud.TagsLayouter
{
    public interface IWordTagsLayouter
    {
        (IReadOnlyList<WordTag>, int) GetWordTagsAndCloudRadius(string text);
    }
}