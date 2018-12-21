using ResultOf;

namespace TagCloud
{
    public interface ITextReader
    {
        Result<string> TryReadText(string fileName);
    }
}