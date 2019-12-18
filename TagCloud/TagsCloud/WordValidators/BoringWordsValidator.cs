using System.Collections.Generic;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.WordValidators
{
    public class BoringWordsValidator : IWordValidator
    {
        private readonly HashSet<string> boringWords;

        public BoringWordsValidator(IEnumerable<string> boringWords)
        {
            this.boringWords = boringWords.ToHashSet();
        }

        public Result<bool> IsValidWord(string word)
        {
            return word == null
                ? Result.Fail<bool>("Word cannot be null")
                : (!boringWords.Contains(word)).AsResult();
        }
    }
}