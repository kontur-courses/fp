using System.Collections.Generic;
using System.Linq;

namespace TagsCloud
{
    public class LowerWords
    {
        private readonly IWordCollection words;

        public LowerWords(IWordCollection words)
        {
            this.words = words;
        }

        public Result<IEnumerable<string>> ToLower()
        {
            var enumerable = words.GetWords();
            return enumerable.Then(x => x.Select(word => word.ToLowerInvariant()));
        }
    }
}