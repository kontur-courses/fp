namespace TagCloud.WordFilters;

public interface IWordFilter
{
    Dictionary<string, int> ExcludeWords(Dictionary<string, int> counts);
}