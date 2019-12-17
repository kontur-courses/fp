using System.Collections.Generic;
using ResultOf;
using TagCloud.Models;

namespace TagCloud
{
    public interface IWordsToTagsParser
    {
        Result<List<Tag>> GetTags(Dictionary<string, int> words, ImageSettings imageSettings);
    }
}