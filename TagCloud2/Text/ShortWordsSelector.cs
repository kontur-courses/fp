using ResultOf;

namespace TagCloud2.Text
{
    public class ShortWordsSelector : ISillyWordSelector
    {
        public Result<bool> IsWordSilly(string word)
        {
            return word.Length <= 3;
        }
    }
}
