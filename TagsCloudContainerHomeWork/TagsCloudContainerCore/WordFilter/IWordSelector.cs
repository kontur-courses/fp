using System.Collections.Generic;

namespace TagsCloudContainerCore.WordFilter;

public interface IWordSelector
{
    public IEnumerable<string> SelectWords(IEnumerable<string> words);
}