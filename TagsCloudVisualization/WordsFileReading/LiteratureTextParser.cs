using System;
using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public class LiteratureTextParser : IParser
    {
        public Result<IEnumerable<string>> ParseText(string text)
        {
            return Result.Of(() => ParseTextNotPure(text));
        }

        private IEnumerable<string> ParseTextNotPure(string text)
        {
            var currentIndex = 0;

            while (currentIndex < text.Length)
            {
                var wordStartPos = text.SkipUntil(currentIndex, IsWordSymbol);
                var afterWordPos = text.SkipUntil(wordStartPos, ch => !IsWordSymbol(ch));

                yield return text.Substring(wordStartPos, afterWordPos - wordStartPos);
                currentIndex = afterWordPos;
            }
        }

        public string GetModeName()
        {
            return "lit";
        }

        private bool IsWordSymbol(char ch)
        {
            return Char.IsLetter(ch);
        }
    }
}
