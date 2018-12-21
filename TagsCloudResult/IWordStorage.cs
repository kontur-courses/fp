using System.Collections.Generic;
using System.Linq;

namespace TagsCloudResult
{
    public interface IWordStorage
    {
        Result<None> Add(string word);
        void AddRange(IEnumerable<string> words);
        IOrderedEnumerable<Word> ToIOrderedEnumerable();
    }
}