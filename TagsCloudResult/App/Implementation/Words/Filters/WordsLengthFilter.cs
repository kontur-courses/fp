using System;
using System.Collections.Generic;
using System.Linq;
using App.Infrastructure.Words.Filters;

namespace App.Implementation.Words.Filters
{
    public class WordsLengthFilter : IFilter
    {
        private readonly int minLength;

        public WordsLengthFilter(int minLength = 3)
        {
            if (minLength < 1)
                throw new ArgumentException("Word length can not be lesser then 1");

            this.minLength = minLength;
        }

        public Result<IEnumerable<string>> FilterWords(Result<IEnumerable<string>> words)
        {
            return words.IsSuccess
                ? Result.Of(() => words.Value.Where(word => word.Length >= minLength))
                : words;
        }
    }
}