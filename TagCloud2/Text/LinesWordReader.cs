using System;
using System.Linq;
using ResultOf;

namespace TagCloud2
{
    public class LinesWordReader : IWordReader
    {
        public Result<string[]> GetUniqueLowercaseWords(string input)
        {
            return input
                .ToLower()
                .Split(Environment.NewLine)
                .Distinct()
                .ToArray();
        }
    }
}
