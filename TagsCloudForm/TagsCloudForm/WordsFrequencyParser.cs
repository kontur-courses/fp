using System.Collections.Generic;
using System.Linq;
using TagsCloudForm.CircularCloudLayouterSettings;

namespace TagsCloudForm
{
    public class WordsFrequencyParser :IWordsFrequencyParser
    {
        public Dictionary<string, int> GetWordsFrequency(IEnumerable<string> lines, LanguageEnum language)
        {
            return lines
                .GroupBy(x => x)
                .Select(x=>(x.Key.ToString(), x.Count()))
                .ToDictionary(x=>x.Item1, x=>x.Item2);
        }
    }
}
