using System.Collections.Generic;
using System.Text.RegularExpressions;
using TagCloud.ResultMonade;

namespace TagCloud.TextParsing
{
    public class TextParser : ITextParser
    {
        private readonly Regex regex = new Regex(@"\w+", RegexOptions.Compiled);

        public Result<List<string>> GetWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.Fail<List<string>>($"Text <{text}> is null or empty");

            var words = new List<string>();

            var matches = regex.Matches(text);

            foreach (Match match in matches)
                words.Add(match.Value);

            return words.Count > 0
                    ? words
                    : Result.Fail<List<string>>($"Text <{text}> can't be parsed");
        }
    }
}
    