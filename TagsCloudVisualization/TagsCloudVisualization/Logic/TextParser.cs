using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Logic
{
    public class TextParser : IParser
    {
        private static readonly char[] Separators = {'\n', '\r'};
        private readonly IBoringWordsProvider boringWordsProvider;

        public TextParser(IBoringWordsProvider boringWordsProvider)
        {
            this.boringWordsProvider = boringWordsProvider;
        }

        public Result<IEnumerable<WordToken>> ParseToTokens(string text)
        {
            return text
                .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.ToLower())
                .Where(IsWordValid)
                .GroupBy(i => i, (word, words) => new {Word = word, Count = words.Count()})
                .Select(wordPair => new WordToken(wordPair.Word, wordPair.Count))
                .ToArray();
        }

        private bool IsWordValid(string word)
        {
            var pattern = new Regex(@".*[\w\d]+.*");
            return pattern.IsMatch(word) && !boringWordsProvider.BoringWords.Contains(word);
        }
    }
}