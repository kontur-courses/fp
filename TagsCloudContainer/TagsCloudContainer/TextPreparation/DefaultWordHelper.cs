using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.TextPreparation
{
    public class DefaultWordHelper : IWordsHelper
    {
        public Result<List<string>> GetAllWordsToVisualize(List<string> words)
        {
            if (words == null)
            {
                return Result.Fail<List<string>>("Words can't be null");
            }

            return Result.Ok(
                SortWordsByPopularity(RemoveAllBoringWords(words.Select(word => word.ToLower()).ToList())));
        }

        private static List<string> RemoveAllBoringWords(List<string> words)
        {
            return words.Where(word => word.Length > 3).ToList();
        }

        private static List<string> SortWordsByPopularity(List<string> words)
        {
            return words.Distinct().OrderByDescending(word => words.Count(w => w.Equals(word))).ToList();
        }
    }
}