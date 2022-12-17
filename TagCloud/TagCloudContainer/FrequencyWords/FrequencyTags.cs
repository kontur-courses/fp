namespace TagCloudContainer.FrequencyWords
{
    public class FrequencyTags : IFrequencyCounter
    {
        public Result<IEnumerable<WordFrequency>> GetWordsFrequency(IEnumerable<string> words)
        {
            return Result.Of(()=> words
                .GroupBy(w => w)
                .Select(word =>
                    new WordFrequency(word.Key, word.Count()))
                .OrderByDescending(x => x.Count))
                .Then(x=>x as IEnumerable<WordFrequency>).RefineError("No words find");
        }
    }
}