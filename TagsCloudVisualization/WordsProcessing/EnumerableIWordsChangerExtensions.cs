using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public static class EnumerableIWordsChangerExtensions
    {
        public static Result<IEnumerable<string>> ChangeWords(this IEnumerable<IWordsChanger> wordsChangers, IEnumerable<string> words)
        {

            var changedWords = Result.Ok(words);
            foreach (var changer in wordsChangers)
            {
                changedWords = changer.ChangeWords(changedWords.Value);
                if (!changedWords.IsSuccess) break;
            }

            return changedWords;
        }
    }
}
