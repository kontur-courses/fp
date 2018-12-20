using System.Collections.Generic;
using System.Linq;

namespace TagsCloud.Words
{
    public class BoringWordsFilter : IBoringWordsCollection
    {
        private readonly Result<List<string>> boringWords;
        private readonly Result<List<string>> words;

        public BoringWordsFilter(Result<List<string>> boringWords, Result<List<string>> words)
        {
            this.boringWords = boringWords;
            this.words = words;
        }

        public Result<List<string>> DeleteBoringWords()
        {
           return boringWords.Then(value =>
            {
                var boringWordSet = new HashSet<string>(value);
                return words.Then(enumerable => enumerable.Where(word => !boringWordSet.Contains(word)).ToList());
            });
        }
    }
}