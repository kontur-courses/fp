using System.Collections.Generic;
using System.Linq;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public class TextParser : ITextParser
    {
        public Result<IEnumerable<string>> GetWords(Result<string> text)
        {
            var separators = new[] {' ', '\n'};
            var letters = text
                .Then(x => string.Concat(
                    x.Where(c => char.IsLetter(c) || separators.Contains(c))
                ));

            var words = letters
                .Then(x => x.Split(separators))
                .Then(x => x.Select(w => w.ToLower()));

            if (!words.IsSuccess) return Result.Fail<IEnumerable<string>>(words.Error);

            return words.GetValueOrThrow().Any()
                ? words
                : Result.Fail<IEnumerable<string>>("Can not parse input text");
        }
    }
}