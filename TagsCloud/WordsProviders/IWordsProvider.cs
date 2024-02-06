using TagsCloud.WordValidators;


namespace TagsCloud.WordsProviders;

public interface IWordsProvider
{
    public Result<Dictionary<string, int>> GetWords();
}