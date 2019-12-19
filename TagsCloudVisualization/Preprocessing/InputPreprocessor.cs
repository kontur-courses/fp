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
            var preprocessedWords = words.AsResult().Then(x => preprocessors
                .Aggregate(x, (current, action) => action.ProcessWords(current))
                .Select(word => new Word(word)));

            return appSettings.Restrictions.AmountOfWordsOnTagCloud >= 0
                ? preprocessedWords.Then(x => x.Take(appSettings.Restrictions.AmountOfWordsOnTagCloud).ToArray())
                : preprocessedWords.Then(x => x.ToArray());
        }
    }
}