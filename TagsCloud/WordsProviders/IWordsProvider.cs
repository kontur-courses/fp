using TagsCloud.WordValidators;
using TagsCloud.Result;


namespace TagsCloud.WordsProviders;

public interface IWordsProvider
{
    public Result<Dictionary<string, int>> GetWords();
}