using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextPreprocessing
{
    public class WordsExtractor
    {
        public Result<IEnumerable<string>> GetWords(string text)
        {
            return Result.Of(() => Regex.Split(text, @"[^\w-]"))
                .Then(words => words.Select(word => Regex.Replace(word, @"[^\w-]", string.Empty))
                    .Where(preparedWord => preparedWord != string.Empty && preparedWord != "-"));
        }
    }
}