using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagCloud
{
    public class TextParser :ITextParcer
    {
        private readonly ITextReader textReader;
        private readonly IWordChanger wordChanger;
        private Result<SimpleWordParser> wordParser;


        public TextParser(ITextReader textReader, SimpleWordChanger simpleWordChanger, Result<SimpleWordParser> parser)
        {
            this.textReader = textReader;
            wordChanger = simpleWordChanger;
            wordParser = parser;
        }

        public Result<List<string>> TryGetWordsFromText(string input)
        {
            var text = textReader.TryReadText(input);
            const string notLetterRegexp = @"[^\'`\-A-Za-z]";
            return text.Then(t =>
                Result.Of(Regex.Split(t, notLetterRegexp, RegexOptions.Compiled)
                    .Where(s=>s.Length > 0)
                    .ToList));
        }


        public Result<Dictionary<string, int>> ParseWords(List<string> words)
        {
            return Result.Of(() =>
            {
                var resultDictionary = new Dictionary<string, int>();
                foreach (var word in words)
                {
                    if (wordParser.GetValueOrThrow().IsValidWord(word))
                    {
                        var changedWord = wordChanger.ChangeWord(word);
                        if (resultDictionary.ContainsKey(changedWord))
                            resultDictionary[changedWord]++;
                        else
                            resultDictionary.Add(changedWord, 1);
                    }
                }
                return resultDictionary;
            }, "Parse Words Does`t Work.");
        }
    }
}