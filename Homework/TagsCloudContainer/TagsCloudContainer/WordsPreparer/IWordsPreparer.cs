using System.Collections.Generic;
using TagsCloudResult;

namespace TagsCloudBuilder
{
    public interface IWordsPreparer
    {
        Result<Dictionary<string, int>> GetPreparedWords();
    }
}
