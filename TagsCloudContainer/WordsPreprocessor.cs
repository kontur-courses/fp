using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordsPreprocessor : IWordsPreprocessor
    {
        private readonly Result<string[]> words;
        private readonly Result<string[]> excludedWords;

        public WordsPreprocessor(ISource source, TextFileReader excludedWordsSource)
        {
            words = source.GetWords();
            excludedWords = excludedWordsSource.GetWords();
        }

        public Result<Dictionary<string, int>> GetWordsFrequency()
        {
            if (!words.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(words.Error);
            if (!excludedWords.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(excludedWords.Error);
            return Result.Ok(GetWordsWithFrequency());
        }

        private Dictionary<string, int> GetWordsWithFrequency()
        {
            return words.Value
                .Select(word => word.ToLower())
                .Distinct()
                .Except(excludedWords.Value, StringComparer.OrdinalIgnoreCase)
                .RemoveBoring()
                .ToDictionary(w => w, w => words.Value
                    .Count(s => string.Equals(s, w, StringComparison.OrdinalIgnoreCase)
                    )
                );
        }
    }
}
