namespace TagsCloudContainer.TextAnalysers;

public class FrequencyCalculator : IFrequencyCalculator
{
    public Result<IEnumerable<WordDetails>> CalculateFrequency(IEnumerable<WordDetails> wordsDetails)
    {
        var wordsDictionary = new Dictionary<string, WordDetails>();
        foreach (var details in wordsDetails)
        {
            if (!wordsDictionary.ContainsKey(details.Word))
                wordsDictionary.TryAdd(details.Word, details);
            else
                wordsDictionary[details.Word].Frequency++;
        }

        return Result.Ok(wordsDictionary
            .Select(pair => pair.Value));
    }
}