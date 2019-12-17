using ResultOF;

namespace TagCloud
{
    public interface IParser : ICheckable
    {
        Result<string[]> ParseWords(string[] words);
    }
}
