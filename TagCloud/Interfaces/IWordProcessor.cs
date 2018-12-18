using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IWordProcessor
    {
        Result<string> Process(string word);
    }
}