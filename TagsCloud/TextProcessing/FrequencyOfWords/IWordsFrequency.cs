using System.Collections.Generic;

namespace TagsCloud.TextProcessing.FrequencyOfWords
{
    public interface IWordsFrequency
    {
        Dictionary<string, int> GetWordsFrequency(string text);
    }
}