using System.Collections.Generic;
using System.Linq;

namespace TagsCloud.Words
{
    public class LowerWords
    {
        private readonly IWordCollection words;

        public LowerWords(IWordCollection words)
        {
            this.words = words;
        }

        public Result<List<string>> ToLower()
        {
            var enumerable = words.GetWords();
            return enumerable.Then(x => x.Select(word => word.ToLowerInvariant()).ToList());
        }
    }
}