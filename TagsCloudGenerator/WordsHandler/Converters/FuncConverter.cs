using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudGenerator.WordsHandler.Converters
{
    public class FuncConverter : IConverter
    {
        private readonly Func<KeyValuePair<string, int>, KeyValuePair<string, int>> convert;

        public FuncConverter(Func<KeyValuePair<string, int>, KeyValuePair<string, int>> convert)
        {
            this.convert = convert;
        }

        public Result<Dictionary<string, int>> Convert(Dictionary<string, int> wordToCount)
        {
            return Result
                .Of(() => ConvertByFunc(wordToCount))
                .RefineError("Couldn't convert by func");
        }

        private Dictionary<string, int> ConvertByFunc(Dictionary<string, int> wordToCount)
        {
            return wordToCount.Select(convert).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}