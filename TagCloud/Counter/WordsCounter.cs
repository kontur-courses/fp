using System.Collections.Generic;
using System.Linq;
using TagCloud.Data;

namespace TagCloud.Counter
{
    public class WordsCounter : IWordsCounter
    {
        public Result<IEnumerable<WordInfo>> GetWordsInfo(IEnumerable<string> words)
        {
            var wordsArray = words as string[] ?? words.ToArray();
            if (wordsArray.Length == 0)
                return Result.Fail<IEnumerable<WordInfo>>("No words were found");
            var occurrences = new Dictionary<string, int>();
            foreach (var word in wordsArray)
            {
                if (occurrences.ContainsKey(word))
                    occurrences[word]++;
                else
                    occurrences[word] = 1;
            }
            var maxOccurrences = occurrences.Max(pair => pair.Value);
            return Result.Ok(occurrences
                .Select(pair => new WordInfo(pair.Key, pair.Value, (float) pair.Value / maxOccurrences))
                .OrderByDescending(info => info.Occurrences)
                .AsEnumerable());
        }
    }
}