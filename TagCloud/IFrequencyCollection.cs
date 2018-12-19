using System.Collections.Generic;

namespace TagsCloud
{
    public interface IFrequencyCollection
    {
        Result<ICollection<KeyValuePair<string, double>>> GetFrequencyCollection(Result<IEnumerable<string>> words);
    }
}