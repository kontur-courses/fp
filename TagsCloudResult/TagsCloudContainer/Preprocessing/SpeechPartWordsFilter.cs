using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Preprocessing
{
    public class SpeechPartWordsFilter : IWordsPreprocessor
    {
        private readonly IWordSpeechPartParser wordSpeechPartParser;
        private readonly ISpeechPartFilterSettings settings;

        public SpeechPartWordsFilter(
            IWordSpeechPartParser wordSpeechPartParser,
            ISpeechPartFilterSettings settings)
        {
            this.wordSpeechPartParser = wordSpeechPartParser;
            this.settings = settings;
        }

        public Result<IEnumerable<string>> Preprocess(IEnumerable<string> words)
        {
            return wordSpeechPartParser.ParseWords(words)
                .Then(speechPartWords => speechPartWords
                    .Where(word => !settings.SpeechPartsToRemove.Contains(word.SpeechPart))
                    .Select(word => word.Word));
        }
    }
}