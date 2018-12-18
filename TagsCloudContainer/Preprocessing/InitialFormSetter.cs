using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;
using ResultOf;

namespace TagsCloudContainer.Preprocessing
{
    public class InitialFormSetter : IWordsPreprocessor
    {
        private readonly WordsPreprocessorSettings settings;

        public InitialFormSetter(WordsPreprocessorSettings settings)
        {
            this.settings = settings;
        }

        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            var separator = Path.DirectorySeparatorChar;
            var result = Result.Of(() => new Hunspell($"NHunspellDictionary{separator}ru_RU.aff",
                    $"NHunspellDictionary{separator}ru_RU.dic"))
                .Then(hunspell => !settings.BringInTheInitialForm
                    ? words
                    : words.Select(word => ToInitialForm(word, hunspell)))
                .RefineError("Can't set words to initial form");
            return result;
        }

        private string ToInitialForm(string word, Hunspell hunspell)
        {
            var firstForm = hunspell.Stem(word).FirstOrDefault();
            return firstForm ?? word;
        }
    }
}