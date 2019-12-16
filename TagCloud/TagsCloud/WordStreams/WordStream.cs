using System.Collections.Generic;
using System.Linq;
using TagsCloud.Interfaces;
using TagsCloud.ErrorHandling;

namespace TagsCloud.WordStreams
{
    public class WordStream : IWordStream
    {
        private readonly IWordHandler wordHandler;
        private readonly ITextSpliter textSpliter;
        private readonly ITextReader fileReader;
        private readonly IWordValidator wordValidator;

        public WordStream(IWordHandler wordHandler, ITextSpliter textSpliter, ITextReader fileReader, IWordValidator wordValidator)
        {
            this.textSpliter = textSpliter;
            this.wordHandler = wordHandler;
            this.fileReader = fileReader;
            this.wordValidator = wordValidator;
        }

        public Result<IEnumerable<string>> GetWords(string path)
        {
            return fileReader.ReadFile(path)
            .Then(text => textSpliter.SplitText(text))
            .Then(words => words.Select(word => wordHandler.ProseccWord(word)))
            .Then(words =>
            {
                var result = new List<string>();
                foreach (var word in words)
                {
                    var isValid = wordValidator.IsValidWord(word);
                    if (!isValid.IsSuccess)
                        return Result.Fail<IEnumerable<string>>(isValid.Error);
                    if (isValid.Value)
                        result.Add(word);
                }
                return Result.Ok(result.AsEnumerable());
            })
            .OnFail(errorMsg => Result.Fail<IEnumerable<string>>(errorMsg));
        }
    }
}
