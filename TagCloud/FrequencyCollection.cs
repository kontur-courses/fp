using System.Collections.Generic;
using System.Linq;

namespace TagsCloud
{
    public class FrequencyCollection : IFrequencyCollection
    {
        public Result<ICollection<KeyValuePair<string, double>>> GetFrequencyCollection(
            Result<IEnumerable<string>> words)
        {
            return words.Then(value =>
            {
                var frequencyDictionary = new Dictionary<string, int>();
                var wordsList = value.ToList();
                foreach (var word in wordsList)
                    if (frequencyDictionary.ContainsKey(word))
                        frequencyDictionary[word]++;
                    else
                        frequencyDictionary[word] = 1;

                var normalizedDictionary = NormalizeDictionary(frequencyDictionary, wordsList.Count);
                var orderedCollection = normalizedDictionary.OrderByDescending(pair => pair.Value);
                return orderedCollection.ToList() as ICollection<KeyValuePair<string, double>>;
            });
        }

        private static ICollection<KeyValuePair<string, double>> NormalizeDictionary(
            Dictionary<string, int> frequencyDictionary,
            int wordsCount)
        {
            var normalizedFrequencyDictionary = new Dictionary<string, double>();
            foreach (var pair in frequencyDictionary)
            {
                var value = pair.Value * 1.0 / wordsCount * 100;
                normalizedFrequencyDictionary.Add(pair.Key, value);
            }

            return normalizedFrequencyDictionary;
        }
    }
}