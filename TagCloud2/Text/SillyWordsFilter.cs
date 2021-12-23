using ResultOf;

namespace TagCloud2.Text
{
    public class SillyWordsFilter : ISillyWordsFilter
    {
        public string FilterSillyWords(string input, ISillyWordSelector selector)
        {
            return selector.IsWordSilly(input) ? "" : input;
        }
    }
}
