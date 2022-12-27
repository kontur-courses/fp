using System.Collections.Generic;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Models;
using TagCloudContainer.Result;

namespace TagCloudContainer.FrequencyWords
{
    public class FrequencyCounter : IFrequencyCounter
    {
        public Result<IEnumerable<TagWithFrequency>> GetTagsFrequency(IEnumerable<string> words)
        {
            if (words == null)
                return Result.Result.Fail<IEnumerable<TagWithFrequency>>("words не может быть null");

            var totalWords = 0;
            var frequencyDict = new Dictionary<string, int>();

            foreach (var word in words)
            {
                totalWords++;

                if (frequencyDict!.ContainsKey(word))
                    frequencyDict[word]++;
                else
                    frequencyDict.Add(word, 1);
            }

            frequencyDict = frequencyDict
                .OrderByDescending(x => x.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                );

            return Result.Result.Ok(frequencyDict
                .Select(pair => new TagWithFrequency(pair.Key, pair.Value)));
        }
    }
}