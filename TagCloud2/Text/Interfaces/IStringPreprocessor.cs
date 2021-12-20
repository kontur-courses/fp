using ResultOf;

namespace TagCloud2
{
    public interface IStringPreprocessor
    {
        Result<string> PreprocessString(string input);
    }
}
