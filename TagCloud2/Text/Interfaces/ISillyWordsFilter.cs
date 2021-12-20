using ResultOf;

namespace TagCloud2
{
    public interface ISillyWordsFilter
    {
        Result<string> FilterSillyWords(string input, ISillyWordSelector selector);
    }
}
