using System.Collections.Generic;
using System.Linq;

namespace TagsCloud
{
    public class BoringWordsFilter : IBoringWordsCollection
    {
        private readonly Result<IEnumerable<string>> boringWords;
        private readonly Result<IEnumerable<string>> words;

        public BoringWordsFilter(Result<IEnumerable<string>> boringWords, Result<IEnumerable<string>> words)
        {
            this.boringWords = boringWords;
            this.words = words;
        }

        public Result<IEnumerable<string>> DeleteBoringWords()
        {
            if (boringWords.IsSuccess)
            {
                var boringWordSet = new HashSet<string>(boringWords.Value);
                return words.Then(enumerable => enumerable.Where(word => !boringWordSet.Contains(word)));
            }

            return Result.Fail<IEnumerable<string>>("Incorrect boring words");
        }
    }
}