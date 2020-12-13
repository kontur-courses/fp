using System.Collections.Generic;
using System.Linq;
using TagsCloudCreating.Configuration;
using TagsCloudCreating.Contracts;

namespace TagsCloudCreating.Core.WordProcessors
{
    public class WordHandler : IWordHandler
    {
        private HashSet<string> BoringTypes { get; }
        private WordHandlerSettings WordHandlerSettings { get; }

        public WordHandler(WordHandlerSettings wordHandlerSettings)
        {
            WordHandlerSettings = wordHandlerSettings;
            BoringTypes = WordHandlerSettings.SpeechPartsStatuses
                .Where(part => !part.Value)
                .Select(part => MyStemHandler.BoringWords[part.Key])
                .ToHashSet();
        }

        public IEnumerable<string> NormalizeAndExcludeBoringWords(IEnumerable<string> words) =>
            MyStemHandler.GetWordsWithPartsOfSpeech(words)
                .Where(ExcludeBoringWords)
                .Select(pair => pair.word);

        private bool ExcludeBoringWords((string word, string speechPart) pair) =>
            !string.IsNullOrEmpty(pair.word) && !BoringTypes.Contains(pair.speechPart) && pair.word.Length > 1;
    }
}