using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Processing
{
    public class WordParser : IParser
    {
        private static readonly char[] Whitespaces = {' ', '\r', '\n', '\t'};
        private readonly ParserSettings settings;

        public WordParser(ParserSettings settings)
        {
            this.settings = settings;
        }

        public Result<Dictionary<string, int>> ParseWords(string input)
        {
            IEnumerable<string> words = string.Concat(input.Where(c => !char.IsPunctuation(c)))
                .Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);

            words = FilterWords(ConvertWords(words));

            var wordCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!wordCount.ContainsKey(word))
                    wordCount[word] = 0;
                wordCount[word]++;
            }

            return Result.Ok(wordCount);
        }

        private IEnumerable<string> ConvertWords(IEnumerable<string> words)
        {
            return settings.Converters.Aggregate(words, (current, converter) => converter.Convert(current));
        }

        private IEnumerable<string> FilterWords(IEnumerable<string> words)
        {
            return settings.Filters.Aggregate(words, (current, filter) => filter.Filter(current));
        }
    }
}