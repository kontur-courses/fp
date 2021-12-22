using System.Collections.Generic;
using TagCloud.ResultMonad;

namespace TagCloud.Creators
{
    public interface ITagCreator
    {
        Result<IEnumerable<Tag>> Create(Dictionary<string, int> wordsWithFrequency);
    }
}
