using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Logic
{
    public class TextParser : IParser
    {
        private static readonly Regex WordPattern = new Regex(@"(?=[\w]['*-]?)([\w'*-]+)");
        private readonly IBoringWordsProvider boringWordsProvider;

        public TextParser(IBoringWordsProvider boringWordsProvider)
        {
            this.boringWordsProvider = boringWordsProvider;
        }

        public Result<IEnumerable<WordToken>> ParseToTokens(string text)
        {
            return string.IsNullOrEmpty(text)
                ? Result.Fail<IEnumerable<WordToken>>("Text was null or empty")
                : SplitToWords(text)
                    .GroupBy(i => i, (word, words) => new {Word = word, Count = words.Count()})
                    .Select(wordPair => new WordToken(wordPair.Word, wordPair.Count))
                    .AsResult();
        }

        private IEnumerable<string> SplitToWords(string text)
        {
            return WordPattern
                .Matches(text)
                .OfType<Match>()
                .Select(match => match.Value.ToLower())
                .Where(word => !IsWordBoring(word));
        }

        private bool IsWordBoring(string word)
        {
            return boringWordsProvider.BoringWords.Contains(word);
        }
    }
}