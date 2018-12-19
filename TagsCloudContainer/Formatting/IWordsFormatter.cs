using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TagsCloudContainer.Formatting
{
    public interface IWordsFormatter
    {
        ReadOnlyCollection<string> Format(IEnumerable<string> words);
    }
}