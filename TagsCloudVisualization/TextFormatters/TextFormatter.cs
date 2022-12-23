using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ResultOf;

namespace TagsCloudVisualization.TextFormatters
{
    public class TextFormatter : ITextFormatter
    {
        public IWordFilter Filter;

        public TextFormatter(IWordFilter filter)
        {
            Filter = filter;
        }

        public Result<List<Word>> Format(string text)
        {
            return GetWords(text).Then(words =>
            {
                words.ForEach(t => t.ToLower());
                return GetAllWordsFromText(words.Where(t => Filter.IsPermitted(t).Value));
            });
        }

        private Result<List<string>> GetWords(string text)
        {
            var words = new List<string>();

            var regex = new Regex(@"\w+");

            var matches = regex.Matches(text);

            foreach (Match match in matches)
                words.Add(match.Value);

            return words;
        }

        private Result<List<Word>> GetAllWordsFromText(IEnumerable<string> words)
        {
            var wordsCount = words.Count();

            return Result.Of(() => words.GroupBy(word => word)
                .OrderByDescending(group => group.Count()).Select(group =>
                {
                    var word = new Word(group.Key)
                    {
                        Frequency = (float)group.Count() / wordsCount
                    };
                    return word;
                }).ToList());
        }
    }
}
