namespace TagsCloudContainer.TextAnalysers.WordsFilters;

public interface IWordsFilter
{
    public Result<IEnumerable<WordDetails>> Filter(IEnumerable<WordDetails> wordDetails);
}