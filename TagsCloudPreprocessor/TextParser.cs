using System.Collections.Generic;
using System.Linq;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public class TextParser : ITextParser
    {
        public Result<IEnumerable<string>> GetWords(string text)
        {
            var separators = new[] {' ', '\n'};

            var letters = string.Concat(
                text.Where(c => char.IsLetter(c) || separators.Contains(c))
            );

            var words = letters
                .Split(separators)
                .Select(w => w.ToLower());

            return words.Any()
                ? words.AsResult()
                : Result.Fail<IEnumerable<string>>("Can not parse input text");
        }
    }
}