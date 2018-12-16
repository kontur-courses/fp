using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Счетчик слов, который отсекает слова, встречающиеся один раз
    /// </summary>
    public class WordCounter
    {
        public Font Font { get; set; } = new Font("Consolas", 14);
        private List<string> stopWords;

        public WordCounter()
        {
            try
            {
                stopWords = Regex.Split(new TxtReader().Read("Stopwords.txt").Value.ToLower(), @"\W+").ToList();
            }
            catch
            {
                stopWords = new List<string>();
            }
        }

        private Result<List<string>> SpellCheck(List<string> words)
        {
            var checkedWords = new List<string>();
            try
            {

                using (var hunspell = new Hunspell("ru.aff", "ru.dic"))
                {
                    foreach (var word in words)
                    {
                        var variants = hunspell.Stem(word);
                        checkedWords.Add(variants.Count == 0 ? word : variants.First());
                    }
                }

                return Result.Ok(checkedWords);
            }
            catch (Exception e)
            {
                return Result.Fail<List<string>>(e.Message);
            }
        }

        public Result<List<GraphicWord>> Count(string row)
        {
            var words = Regex.Split(row.ToLower(), @"\W+").ToList();

            var result = SpellCheck(words);
            if (result.IsSuccess)
                words = result.Value;

            var countedWords = new Dictionary<string, GraphicWord>();
            foreach (var word in words)
            {
                if (!countedWords.ContainsKey(word))
                    countedWords[word] = new GraphicWord(word);
                else
                    countedWords[word].Rate++;
            }

            foreach (var dictValue in countedWords.Values)
            {
                SetFontSize(dictValue);
            }

            return Result.Ok(countedWords
                .Values
                .Where(w => w.Rate > 1 && !stopWords.Contains(w.Value))
                .OrderByDescending(w => w.Rate).ToList());
        }

        private void SetFontSize(GraphicWord word)
        {
            word.Font = new Font(Font.Name, word.Rate + Font.Size);
        }
    }
}
