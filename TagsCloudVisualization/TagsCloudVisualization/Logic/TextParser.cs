using System;
using System.Collections.Generic;
using System.Linq;
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
            if (text == null)
                return Result.Fail<IEnumerable<WordToken>>("Text is null");
            var wordCountDictionary = new Dictionary<string, int>();
            var splittedText = text
                .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.ToLower());
            foreach (var lineWord in splittedText)
            {
                if (IsWordInvalid(lineWord))
                    continue;
                if (!wordCountDictionary.ContainsKey(lineWord))
                    wordCountDictionary.Add(lineWord, 1);
                else
                    wordCountDictionary[lineWord] += 1;
            }
            return wordCountDictionary.Select(kvp => new WordToken(kvp.Key, kvp.Value)).ToArray();
        }

        private bool IsWordInvalid(string word)
        {
            return boringWordsProvider.BoringWords != null
                   && boringWordsProvider.BoringWords.Contains(word)
                   || string.IsNullOrEmpty(word);
        }
    }
}