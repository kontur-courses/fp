using System.Collections.Generic;

namespace TagsCloud.Words
{
    public interface IFrequencyCollection
    {
        Result<List<KeyValuePair<string, double>>> GetFrequencyCollection(Result<IEnumerable<string>> words);
    }
}