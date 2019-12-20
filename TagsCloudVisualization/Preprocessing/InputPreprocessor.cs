using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Preprocessing
{
    public class InputPreprocessor
    {
        private readonly AppSettings appSettings;
        private readonly IPreprocessor[] preprocessors;

        public InputPreprocessor(IPreprocessor[] preprocessors, AppSettings appSettings)
        {
            this.preprocessors = preprocessors;
            this.appSettings = appSettings;
        }

        public Result<Word[]> PreprocessWords(IEnumerable<string> words)
        {
            return Result.Of(() => GetPreprocessedWords(words).ToArray());
        }

        private IEnumerable<Word> GetPreprocessedWords(IEnumerable<string> words)
        {
            var preprocessedWords = preprocessors.Aggregate(words, (current, action) => action.ProcessWords(current))
                .Select(word => new Word(word));

            return appSettings.Restrictions.AmountOfWordsOnTagCloud >= 0
                ? preprocessedWords.Take(appSettings.Restrictions.AmountOfWordsOnTagCloud)
                : preprocessedWords;
        }
    }
}