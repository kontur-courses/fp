using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IWordFilter
    {
        Result<bool> ToExclude(string word);
    }
}