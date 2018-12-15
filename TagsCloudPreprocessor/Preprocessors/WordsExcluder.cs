using System.Collections.Generic;
using System.Linq;
using ResultOfTask;

namespace TagsCloudPreprocessor.Preprocessors
{
    public class WordsExcluder : IPreprocessor
    {
        private readonly IWordExcluder wordExcluder;

        public WordsExcluder(IWordExcluder wordExcluder)
        {
            this.wordExcluder = wordExcluder;
        }

        private Result<HashSet<string>> GetForbiddenWords()
        {
            return wordExcluder.GetExcludedWords();
        }

        private Result<IEnumerable<string>> ExcludeForbiddenWords(Result<List<string>> words)
        {
            var forbiddenWordsResult = GetForbiddenWords();

            if (!forbiddenWordsResult.IsSuccess)
                return Result.Fail<IEnumerable<string>>("Can not load dictionary with excluded words");

            var forbiddenWords = forbiddenWordsResult.GetValueOrThrow();
            
            return words.Then(x => x.Where(w => !forbiddenWords.Contains(w)));
        }

        public Result<List<string>> PreprocessWords(Result<List<string>> words)
        {
            var validWords = ExcludeForbiddenWords(words)
                .Then(x => x.ToList());
            
            if (!validWords.IsSuccess) return Result.Fail<List<string>>(validWords.Error);
            
            return validWords.GetValueOrThrow().Any()
                ? validWords
                : Result.Fail<List<string>>("Can not exclude input text");
        }
    }
}