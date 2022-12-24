using System.Collections.Generic;
using System.Linq;
using TagCloudContainer.TaskResult;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    /// <summary>
    /// Обрабатывает входной файл. На выходе - частотный словарь без скучных слов.
    /// </summary>
    public static class InputFileHandler
    {
        public static Result<Dictionary<string, int>> FormFrequencyDictionary(IEnumerable<string> words)
        {
            var filteredWords = BoringWordsDeleter.DeleteBoringWords(words.Select(word => word.ToLower()));
            return filteredWords.Then(array =>
                Result.OnSuccess(
                    array
                        .GroupBy(w => w)
                        .ToDictionary(g => g.Key, g => g.Count())));
        }
    }
}