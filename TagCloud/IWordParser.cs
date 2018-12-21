using ResultOf;

namespace TagCloud
{
    public interface IWordParser
    {
        bool IsValidWord(string word);
    }
}