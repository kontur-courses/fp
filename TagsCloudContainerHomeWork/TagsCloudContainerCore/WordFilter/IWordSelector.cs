using System.Collections.Generic;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.WordFilter;

public interface IWordSelector
{
    public Result<IEnumerable<string>> SelectWords(IEnumerable<string> words);
}