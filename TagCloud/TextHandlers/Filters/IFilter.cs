namespace TagCloud.TextHandlers.Filters
{
    public interface IFilter
    {
        bool IsCorrectWord(string word);
    }
}