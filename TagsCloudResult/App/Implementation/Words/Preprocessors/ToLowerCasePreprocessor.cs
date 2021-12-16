using System.Collections.Generic;
using System.Linq;
using App.Infrastructure.Words.Preprocessors;

namespace App.Implementation.Words.Preprocessors
{
    public class ToLowerCasePreprocessor : IPreprocessor
    {
        public Result<IEnumerable<string>> Preprocess(Result<IEnumerable<string>> words)
        {
            return words.IsSuccess
                ? Result.Of(() => words.Value.Select(word => word.ToLower()))
                : words;
        }
    }
}