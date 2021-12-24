using System.Collections.Generic;

namespace TagCloud2
{
    public interface IWordReader
    {
        IEnumerable<string> GetUniqueLowercaseWords(string input);
    }
}
