using System.Collections.Generic;
using System.Linq;
using TagCloudContainer.Result;
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
            if (!filteredWords.IsSuccess)
                return new Result<Dictionary<string, int>>(filteredWords.Error);
            var filteredWordsToDictionary = filteredWords.Value.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            return new Result<Dictionary<string, int>>(null, filteredWordsToDictionary);
        }
    }
}