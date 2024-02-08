namespace TagCloudResult.TextProcessing
{
    public class TextProcessor(Settings settings, ITextReader fileTextReader) : ITextProcessor
    {
        public Result<Dictionary<string, int>> GetWordsFrequency()
        {
            var excludedWordsResult = fileTextReader.GetWordsFrom(settings.ExcludedWordsPath)
                .RefineError("Can't read excluded words");
            if (!excludedWordsResult.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(excludedWordsResult.Error);

            var wordsResult = fileTextReader.GetWordsFrom(settings.TextPath)
                .RefineError("Can't read words");
            if (!wordsResult.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(wordsResult.Error);

            var excludedWords = excludedWordsResult.Value.ToHashSet();
            return Result.Ok(fileTextReader.GetWordsFrom(settings.TextPath).Value
                .Where(t => t.Length > 3 && !excludedWords.Contains(t))
                .GroupBy(x => x)
                .ToDictionary(key => key.Key, amount => amount.Count()));
        }
    }
}
