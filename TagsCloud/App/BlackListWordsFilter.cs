using System.Collections.Generic;
using System.Linq;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public class BlackListWordsFilter : IWordsFilter
    {
        internal readonly HashSet<string> BlackList;
        private readonly IWordNormalizer normalizer;

        public BlackListWordsFilter(IEnumerable<string> excludedWords, IWordNormalizer normalizer)
        {
            BlackList = excludedWords.Select(normalizer.Normalize).ToHashSet();
            this.normalizer = normalizer;
        }

        public bool Validate(string word)
        {
            return !BlackList.Contains(normalizer.Normalize(word));
        }

        public Result<None> UpdateBlackList(IEnumerable<string> words)
        {
            return Result.OfAction(() => TryUpdateBlackList(words));
        }

        private Result<None> TryUpdateBlackList(IEnumerable<string> words)
        {
            if (words == null)
                return Result.Fail<None>("Words collection should not be null");
            return Result.OfAction(() =>
            {
                BlackList.Clear();
                foreach (var word in words)
                    BlackList.Add(normalizer.Normalize(word));
            });
        }
    }
}