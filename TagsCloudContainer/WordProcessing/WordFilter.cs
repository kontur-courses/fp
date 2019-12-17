using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    class WordFilter : IWordFilter
    {
        private readonly Func<string, bool> filterFunc;
        
        public WordFilter(Func<string, bool> filterFunc)
        {
            this.filterFunc = filterFunc;
        }

        public WordFilter() : this(x => x.Length > 2)
        {
        }

        public Result<IEnumerable<string>> Filter(IEnumerable<string> words)
        {
            var filtered =  words.Where(word => filterFunc(word)).ToList();
            return filtered.Count == 0 
                ? Result.Fail<IEnumerable<string>>("No wrods left after filtering") 
                : filtered;
        }
    }
}