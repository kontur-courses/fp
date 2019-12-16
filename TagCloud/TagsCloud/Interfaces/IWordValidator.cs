using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IWordValidator
    {
        Result<bool> IsValidWord(string word);
    }
}
