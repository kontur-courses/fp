using System.Collections.Generic;
using System.Linq;

namespace TagsCloudGenerator.WordsHandler.Converters
{
    public class LowercaseConverter : IConverter
    {
        public Result<Dictionary<string, int>> Convert(Dictionary<string, int> wordToCount)
        {
            return Result
                .Of(() => ConvertToLower(wordToCount))
                .RefineError("Couldn't convert to lowercase");
        }

        private static Dictionary<string, int> ConvertToLower(Dictionary<string, int> wordToCount)
        {
            return wordToCount
                .GroupBy(x => x.Key.ToLower())
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
        }
    }
}