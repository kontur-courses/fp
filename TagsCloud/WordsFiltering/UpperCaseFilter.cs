using System.Collections.Immutable;
using System.Linq;

namespace TagsCloud.WordsFiltering
{
    public class UpperCaseFilter : IFilter
    {
        public Result<ImmutableList<string>> FilterWords(ImmutableList<string> words)
        {
            return Result.Of(() => ImmutableList.ToImmutableList(words.Select(w => w.ToUpper())));
        }
    }
}
