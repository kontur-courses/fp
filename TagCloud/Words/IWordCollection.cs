using System.Collections.Generic;

namespace TagsCloud.Words
{
    public interface IWordCollection
    {
        Result<List<string>> GetWords();
    }
}