using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Common;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer
{
    public class WordsCountParser
    {
        public List<SimpleTag> Parse(string str)
        {
            var wordsCount = new Dictionary<string, int>();
            var words = str.Split("\r\n")
                .Where(w => w.Length != 0);
            foreach (var word in words) 
                wordsCount.AddOrIncreaseCount(word);
            var tags = wordsCount.Keys
                .Select(word => new SimpleTag(word, wordsCount[word]))
                .ToList();
            if (tags.Count == 0)
                throw new Exception("Parser not find any words, you can`t visualize it");
            return tags;
        }
    }
}