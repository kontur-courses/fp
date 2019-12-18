using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.WordStreams
{
    public class WordStream
    {
        private readonly ITextReader fileReader;
        private readonly ITextSplitter textSplitter;
        private readonly IWordHandler wordHandler;
        private readonly List<IWordValidator> wordValidators;

        public WordStream(IWordHandler wordHandler, ITextSplitter textSplitter, ITextReader fileReader,
            IEnumerable<IWordValidator> wordValidators)
        {
            this.textSplitter = textSplitter;
            this.wordHandler = wordHandler;
            this.fileReader = fileReader;
            this.wordValidators = wordValidators.ToList();
        }

        public Result<None> AddNewValidator(IWordValidator wordValidator)
        {
            return Result.OfAction(() => wordValidators.Add(wordValidator));
        }

        public Result<IEnumerable<string>> GetWords(string path)
        {
            return fileReader.ReadFile(path)
                .Then(text => textSplitter.SplitText(text))
                .Then(words => words.Select(wordHandler.ProcessWord))
                .Then(GetValidWords);
        }

        private Result<IEnumerable<string>> GetValidWords(IEnumerable<string> words)
        {
            var result = new List<string>(words);
            var errors = new List<string>();
            var validWords = wordValidators.Aggregate(result, (current, wordValidator) => current.Where(word =>
                {
                    var isValid = wordValidator.IsValidWord(word)
                        .OnFail(errorMsg => errors.Add(word + ": " + errorMsg));
                    return isValid.IsSuccess && isValid.Value;
                })
                .ToList());
            return errors.Count != 0
                ? Result.Fail<IEnumerable<string>>(string.Join(Environment.NewLine, errors))
                : Result.Ok(validWords.AsEnumerable());
        }
    }
}