namespace TagCloud.TextHandlers.Filters;

internal class BoringWordsFilter : IFilter
{
    public bool IsCorrectWord(string word)
    {
        return word.Length > 3;
    }
}