using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.Splitters
{
    public class SplitterByWhiteSpace : ITextSplitter
    {
        private readonly char[] splitChar = {',', '.', '!', '?', ';', ':', ' '};

        public Result<IEnumerable<string>> SplitText(string text)
        {
            if (text == null)
                return Result.Fail<IEnumerable<string>>("Text is null");
            return Result.Of(() => text.Split(Environment.NewLine))
                .Then(lines => lines.SelectMany(line => line.Split(splitChar)))
                .Then(words => words.Select(RemoveExtraCharacters))
                .Then(words => words.Where(word => !string.IsNullOrEmpty(word)));
        }

        private static string RemoveExtraCharacters(string word)
        {
            return Regex.Replace(word, @"[^\w\.@-]", "", RegexOptions.None);
        }
    }
}