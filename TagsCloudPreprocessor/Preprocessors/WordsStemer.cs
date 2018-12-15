using System.Collections.Generic;
using System.Linq;
using NHunspell;
using ResultOfTask;

namespace TagsCloudPreprocessor.Preprocessors
{
    public class WordsStemer : IPreprocessor
    {
        public Result<List<string>> PreprocessWords(Result<List<string>> words)
        {
            return GetWordsStem(words).Then(x => x.ToList());
        }

        private Result<List<string>> GetWordsStem(Result<List<string>> words)
        {
            var hunspellResult = Result.Of(() => new Hunspell(@"ru_RU.aff", @"ru_RU.dic"));

            if (!hunspellResult.IsSuccess)
                return Result.Fail<List<string>>("Can not load hunspell dictionaries");

            var h = hunspellResult.GetValueOrThrow();


            var stems = words
                .Then(x => x.Select(word => h.Stem(word)))
                .Then(x => x.Select(stem => stem.FirstOrDefault()))
                .Then(x => x.Where(wordStem => wordStem != null))
                .Then(x => x.ToList());

            if (!stems.IsSuccess) return Result.Fail<List<string>>(stems.Error);
            
            return stems.GetValueOrThrow().Any()
                ? stems
                : Result.Fail<List<string>>("Can not stem input text");
        }
    }
}