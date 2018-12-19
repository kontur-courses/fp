using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.TextParsers
{
    public class TextParser : ITextParser
    {
        private readonly TextSettings textSettings;

        public TextParser(TextSettings textSettings)
        {
            this.textSettings = textSettings;
        }


        public Result<List<WordFrequency>> Parse(Result<string> text)
        {
            var words = text.Value.Split(Environment.NewLine.ToCharArray())
                .Select(word => word.Trim())
                .Where(word => !string.IsNullOrEmpty(word));

            var convertedWords = new List<string>();
            foreach (var word in words)
            {
                var convertedWord = word;
                if (textSettings.WordFilters.Any(filter => !filter.Validate(word)))
                    continue;
                foreach (var converter in textSettings.WordConverters)
                {
                    var resultConverted = converter.Convert(word);
                    if (!resultConverted.IsSuccess)
                        return Result.Fail<List<WordFrequency>>(resultConverted.Error);
                    convertedWord = converter.Convert(word).Value;
                }

                convertedWords.Add(convertedWord);
            }

            return convertedWords
                .GroupBy(word => word)
                .Select(group => new WordFrequency(group.Key, group.Count()))
                .OrderByDescending(miniTag => miniTag.Frequency)
                .ThenBy(miniTag => miniTag.Word)
                .Take(textSettings.CountWords)
                .ToList();
        }
    }
}