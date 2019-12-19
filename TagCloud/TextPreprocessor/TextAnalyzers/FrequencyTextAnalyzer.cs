using System.Collections.Generic;
using System.Linq;
using TagCloud.TextPreprocessor.Core;
using TagsCloud;

namespace TagCloud.TextPreprocessor.TextAnalyzers
{
    public class FrequencyTextAnalyzer : ITextAnalyzer
    {
        public Result<IEnumerable<TagInfo>> GetTagInfo(IEnumerable<Tag> tags)
        {
            var frequencyDictionary = new Dictionary<Tag, int>();
            
            foreach (var tag in tags)
            {
                if (frequencyDictionary.ContainsKey(tag))
                    frequencyDictionary[tag] += 1;
                else
                    frequencyDictionary[tag] = 1;
            }

            var tagInfos = frequencyDictionary.Keys
                .Select(word => new TagInfo(word, frequencyDictionary[word]));

            return Result.Ok(tagInfos);
        }
    }
}