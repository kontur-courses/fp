namespace TagsCloud.Visualization.WordsFilters
{
    public class WordLengthFilter : IWordsFilter
    {
        public bool IsWordValid(string word) => word.Length > 1;
    }
}