using TagsCloud.Result;

namespace TagsCloud.WordValidators;

public class SimpleWordValidator : IWordValidator
{
    public Result<bool> IsWordValid(string word)
    {
        var isValid =  word.Length > 3;
        return Result.Result.Ok<bool>(isValid);
    }
}