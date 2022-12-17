namespace TagCloudContainer.FrequencyWords
{
    public interface IFrequencyCounter
    {
        Result<IEnumerable<WordFrequency>> GetWordsFrequency(IEnumerable<string> words);
    }
}
