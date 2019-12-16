namespace TextConfiguration.WordProcessors
{
    public class ToLowerCaseProcessor : IWordProcessor
    {
        public Result<string> ProcessWord(string word)
        {
            return word.ToLower();
        }
    }
}
