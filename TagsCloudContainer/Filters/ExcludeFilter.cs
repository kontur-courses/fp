using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Filters;

public class ExcludeFilter : IFilter
{
    private readonly HashSet<string> wordsToExclude = new()
    {
        "and",
        "to",
        "the",
        "also",
        "a",
        "of"
    };

    public bool Allows(string word)
    {
        return !wordsToExclude.Contains(word);
    }
}
