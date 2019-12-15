using FailuresProcessing;
using System.Linq;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.WordsConverters
{
    [Priority(10)]
    [Factorial("WordsToLowerConverter")]
    public class WordsToLowerConverter : IWordsConverter
    {
        public Result<string[]> Execute(string[] input)
        {
            return Result.Ok(input.Select(w => w.ToLower()).ToArray());
        }
    }
}