using System.Collections.Immutable;

namespace TagsCloud.WordsFiltering
{
    public interface IFilter
    {
        Result<ImmutableList<string>> FilterWords(ImmutableList<string> words);
    }
}
