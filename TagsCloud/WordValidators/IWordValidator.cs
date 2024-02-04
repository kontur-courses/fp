using TagsCloud.Result;

namespace TagsCloud.WordValidators;

public interface IWordValidator
{
    public Result<bool> IsWordValid(string word);
}