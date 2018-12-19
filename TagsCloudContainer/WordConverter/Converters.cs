using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.WordConverter
{
    public class Converters
    {
        private static readonly Dictionary<string, Func<IWordConverter>> WordConvertersDictionary =
            new Dictionary<string, Func<IWordConverter>>
            {
                {"simple", () => new InitialFormWordConverter()}
            };

        public static Result<IWordConverter[]> GetConvertersByName(IEnumerable<string> wordConverters)
        {
            foreach (var wordConverter in wordConverters)
                if (!WordConvertersDictionary.ContainsKey(wordConverter))
                    return Result.Fail<IWordConverter[]>($"cant find word converter {wordConverter}");
            return wordConverters.Select(converter => WordConvertersDictionary[converter]()).ToArray();
        }
    }
}