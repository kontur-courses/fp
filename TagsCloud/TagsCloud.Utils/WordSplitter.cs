using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloud.Utils
{
    public static class WordSplitter
    {
        private static readonly Regex WordPattern = new(@"\b[\w']*\b", RegexOptions.Compiled);

        public static IEnumerable<string> Split(string input)
        {
            var matches = WordPattern.Matches(input);
            return matches.Where(m => !string.IsNullOrEmpty(m.Value)).Select(m => m.Value);
        }
    }
}