using ResultOf;

namespace TagCloud2
{
    public interface IWordReader
    {
        Result<string[]> GetUniqueLowercaseWords(string input);
    }
}
