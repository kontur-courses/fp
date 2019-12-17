using ResultOF;

namespace TagCloud
{
    public interface IFilter : ICheckable
    {
        Result<string[]> FilterWords(string[] words);
    }
}
