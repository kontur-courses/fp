namespace TextConfiguration.WordFilters
{
    public interface IWordFilter
    {
        Result<bool> ShouldExclude(string word);
    }
}
