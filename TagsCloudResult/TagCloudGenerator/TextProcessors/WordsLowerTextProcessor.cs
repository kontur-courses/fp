namespace TagCloudGenerator.TextProcessors
{
    public class WordsLowerTextProcessor : ITextProcessor
    {
        public Result<IEnumerable<string>> ProcessText(IEnumerable<string> text)
        {
            var words = new List<string>();

            foreach (string line in text)
                words.Add(line.ToLower());

            return Result<IEnumerable<string>>.Success(words);
        }
    }
}