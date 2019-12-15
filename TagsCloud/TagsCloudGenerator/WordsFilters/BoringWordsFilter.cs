using FailuresProcessing;
using System.Collections.Generic;
using System.Linq;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.WordsFilters
{
    [Priority(10)]
    [Factorial("BoringWordsFilter")]
    public class BoringWordsFilter : IWordsFilter
    {
        private readonly HashSet<string> wordsToExclude;

        public BoringWordsFilter() =>
            wordsToExclude = new HashSet<string>
            {
                "и", "но", "если", "когда", "или",
                "я", "он", "она", "оно", "они"
            };

        public Result<string[]> Execute(string[] input)
        {
            return Result.Ok(input.Where(s => !wordsToExclude.Contains(s)).ToArray());
        }
    }
}