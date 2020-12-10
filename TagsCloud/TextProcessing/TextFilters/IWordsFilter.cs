namespace TagsCloud.TextProcessing.TextFilters
{
    public interface IWordsFilter
    {
        string[] GetInterestingWords(string[] words);
    }
}