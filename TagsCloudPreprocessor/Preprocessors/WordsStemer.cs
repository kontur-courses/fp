using System.Collections.Generic;
using System.Linq;
using NHunspell;
using ResultOfTask;

namespace TagsCloudPreprocessor.Preprocessors
{
    public class WordsStemer : IPreprocessor
    {
        public Result<List<string>> PreprocessWords(List<string> words)
        {
            return GetWordsStem(words).Then(x => x.ToList());
        }

        private Result<List<string>> GetWordsStem(List<string> words)
        {
            var hunspellResult = Result.Of(() => new Hunspell(@"ru_RU.aff", @"ru_RU.dic"));

            if (!hunspellResult.IsSuccess)
                return Result.Fail<List<string>>("Can not load hunspell dictionaries");

            var h = hunspellResult.GetValueOrThrow();


            var stems = words.Select(word => h.Stem(word))
                .Select(stem => stem.FirstOrDefault())
                .Where(wordStem => wordStem != null)
                .ToList();

            return stems.Any()
                ? stems.AsResult()
                : Result.Fail<List<string>>("Can not stem input text");
        }
    }
}