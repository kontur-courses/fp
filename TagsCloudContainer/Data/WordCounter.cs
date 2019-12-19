using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data
{
    internal static class WordCounter
    {
        internal static Result<IEnumerable<Word>> Count(string[] words)
        {
            return CountOccurrences(words)
                .Select(pair => new Word(pair.Key, pair.Value, (double) pair.Value / words.Length))
                .OrderByDescending(word => word.Occurrences)
                .Cast<Word>()
                .AsResult();
        }

        private static IDictionary<string, int> CountOccurrences(IEnumerable<string> words)
        {
            var occurrences = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (occurrences.ContainsKey(word))
                    occurrences[word]++;
                else
                    occurrences[word] = 1;
            }

            return occurrences;
        }
    }
}