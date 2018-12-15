using System.Collections.Generic;
using System.Linq;
using Result;

namespace TagCloudCreation
{
    public static class WordPreparerExtensions
    {
        public static Result<IEnumerable<string>> PrepareWords(
            this IWordPreparer preparer,
            IEnumerable<string> words,
            TagCloudCreationOptions options)
        {
            return words.Select(word => preparer.PrepareWord(word, options)
                                                .Then(w => w.Unwrap("")))
                        .AsResult()
                        .Then(i => i.Where(w => w != ""));
        }
    }
}
