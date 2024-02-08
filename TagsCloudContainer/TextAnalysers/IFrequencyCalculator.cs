namespace TagsCloudContainer.TextAnalysers;

public interface IFrequencyCalculator
{
    public Result<IEnumerable<WordDetails>> CalculateFrequency(IEnumerable<WordDetails> wordsDetails);
}