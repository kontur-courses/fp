using System.Collections.Generic;
using System.Linq;
using NHunspell;

namespace TagsCloudGenerator.WordsHandler.Converters
{
    public class InitialFormConverter : IConverter
    {
        private readonly string pathToAff;
        private readonly string pathToDictionary;

        public InitialFormConverter(string pathToAff, string pathToDictionary)
        {
            this.pathToAff = pathToAff;
            this.pathToDictionary = pathToDictionary;
        }

        public Result<Dictionary<string, int>> Convert(Dictionary<string, int> wordToCount)
        {
            return Result
                .Of(() => ConvertToInitial(wordToCount))
                .RefineError($"Couldn't convert to initial form");
        }

        private Dictionary<string, int> ConvertToInitial(Dictionary<string, int> wordToCount)
        {
            using (var hunspell = new Hunspell(pathToAff, pathToDictionary))
            {
                return wordToCount
                    .GroupBy(word => hunspell.Stem(word.Key).LastOrDefault())
                    .Where(e => e.Key != null)
                    .ToDictionary(e => e.Key, e => e.Sum(k => k.Value));
            }
        }
    }
}