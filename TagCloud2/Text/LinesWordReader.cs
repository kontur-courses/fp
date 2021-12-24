using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud2
{
    public class LinesWordReader : IWordReader
    {
        public IEnumerable<string> GetUniqueLowercaseWords(string input)
        {
            return input
                .ToLower()
                .Split(Environment.NewLine)
                .Distinct();
        }
    }
}
