using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagCloud.TagCloudVisualization.Analyzer
{
    public class WordAnalyzer : IWordAnalyzer
    {
        public Dictionary<string, int> WeightWords(IEnumerable<string> words)
        {
            return words.GroupBy(w => w)
                .OrderByDescending(word => word.Count())
                .Take(100)
                .ToDictionary(w => w.Key, w => w.Count());
        }

        public IEnumerable<string> SplitWords(string text)
        {
            var wordWithoutSpecialSymbols = RemoveSpecialSymbols(text);
            return wordWithoutSpecialSymbols.Split(',', ' ').Where(p => p.Any());
        }

        public string RemoveSpecialSymbols(string text)
        {
            var textWithoutSpecialSymbols = Regex.Replace(text, "[^\\w\\._]", " ");
            return Regex.Replace(textWithoutSpecialSymbols, @"\s+", " ");
        }
    }
}