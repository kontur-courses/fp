using System.Collections.Generic;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.Filter
{
    public interface IFilter
    {
        Result<IEnumerable<string>> FilterOut(IEnumerable<string> words);
    }
}