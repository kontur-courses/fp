using System.Collections.Generic;
using System.Linq;
using Functional;

namespace TagCloudCreation
{
    public static class WordPreparerExtensions
    {
        public static Result<IEnumerable<string>> Prepare(
            this IWordPreparer preparer,
            IEnumerable<string> words,
            TagCloudCreationOptions options)
        {
            return words.Select(word => preparer.PrepareWord(word, options)
                                                .Then(w => w.Unwrap("")))
                        .AsResultSilently()
                        .Then(i => i.Where(w => w != ""));
        }
    }
}