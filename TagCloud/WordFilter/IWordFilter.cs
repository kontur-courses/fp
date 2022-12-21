using TagCloud.ResultMonade;

namespace TagCloud.WordFilter
{
    public interface IWordFilter
    {
        Result<bool> IsPermitted(string word);
    }
}
