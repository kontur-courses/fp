using ResultOf;

namespace TagCloud2.Text
{
    public class SillyWordsFilter : ISillyWordsFilter
    {
        public Result<string> FilterSillyWords(string input, ISillyWordSelector selector)
        {
            if (selector.IsWordSilly(input))
            {
                return "";
            }
            return input;
        }
    }
}
