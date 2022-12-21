using System.Collections.Generic;
using System.Linq;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    /// <summary>
    /// Обрабатывает входной файл. На выходе - частотный словарь без скучных слов.
    /// </summary>
    public static class InputFileHandler
    {
        public static Dictionary<string, int> FormFrequencyDictionary(IEnumerable<string> words, IUi settings)
        {
            var filteredWords = BoringWordsDeleter.DeleteBoringWords(words, settings);
            return filteredWords.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
        }
    }
}