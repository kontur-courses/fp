namespace TagCloudResult.TextProcessing
{
    public class TextProcessor(Settings settings, ITextReader fileTextReader) : ITextProcessor
    {
        public Result<Dictionary<string, int>> GetWordsFrequency()
        {
            var excludedWordsResult = fileTextReader.GetWordsFrom(settings.ExcludedWordsPath)
                .RefineError("Can't read excluded words");
            if (!excludedWordsResult.IsSuccess)
                return excludedWordsResult.Fail<Dictionary<string, int>>();

            var wordsResult = fileTextReader.GetWordsFrom(settings.TextPath)
                .RefineError("Can't read words");
            if (!wordsResult.IsSuccess)
                return wordsResult.Fail<Dictionary<string, int>>();

            var excludedWords = excludedWordsResult.Value.ToHashSet();
            return fileTextReader.GetWordsFrom(settings.TextPath).Value
                .Where(t => t.Length > 3 && !excludedWords.Contains(t))
                .GroupBy(x => x)
                .ToDictionary(key => key.Key, amount => amount.Count());
        }
    }
}
