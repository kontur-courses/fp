namespace TagCloud.TextHandlers.Filters;

public class BoringWordsFilter : IFilter
{
    public bool IsCorrectWord(string word)
    {
        return word.Length > 3;
    }
}