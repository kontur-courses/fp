using System.Collections.Generic;
using TagsCloudForm.Common;
using TagsCloudForm.CircularCloudLayouterSettings;

namespace TagsCloudForm.WordFilters
{
    public interface IWordsFilter
    {
        Result<IEnumerable<string>> Filter(ICircularCloudLayouterWithWordsSettings settings, IEnumerable<string> words);
    }
}
