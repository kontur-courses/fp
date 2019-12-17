using ResultOF;

namespace TagCloud
{
    public interface IExtractor
    {
        Result<string[]> ExtractWords(string text);
    }
}
