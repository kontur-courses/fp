using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudContainer
{
    public class TextHandler
    {
        private static readonly Regex wordPattern = new Regex(@"\b[a-zA-Z]+", RegexOptions.Compiled);
        private readonly IDullWordsEliminator dullWordsEliminator;
        private readonly ITextReader textReader;

        public TextHandler(ITextReader textReader, IDullWordsEliminator dullWordsEliminator)
        {
            this.dullWordsEliminator = dullWordsEliminator;
            this.textReader = textReader;
        }

        public Result<Dictionary<string, int>> GetWordsFrequencyDict()
        {
            var linesResult = textReader.GetLines();
            if (!linesResult.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(linesResult.Error);
            return Result.Ok(linesResult.Value
                .SelectMany(l => wordPattern.Matches(l).Cast<Match>().Select(m => m.Value))
                .Where(cw => !dullWordsEliminator.IsDull(cw))
                .GroupBy(cw => cw.ToLower())
                .ToDictionary(wg => wg.Key, wg => wg.Count()));
        }
    }
}