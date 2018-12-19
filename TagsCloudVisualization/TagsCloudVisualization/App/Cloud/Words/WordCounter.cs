using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;
using TagsCloudVisualization.App.Cloud.Words;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Счетчик слов, который отсекает слова, встречающиеся один раз
    /// </summary>
    public class WordCounter : IWordCounter
    {
        public Font Font { get; set; } = new Font("Consolas", 14);

        private Result<List<string>> GetStopWords()
        {
            try
            {
                return Regex.Split(new TxtReader().Read("Stopwords.txt").Value.ToLower(), @"\W+").ToList();
            }
            catch (Exception e)
            {
                return Result.Fail<List<string>>(e.Message);
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

                return checkedWords;
            }
            catch (Exception e)
            {
                return Result.Fail<List<string>>(e.Message);
            }
        }

        public Result<List<GraphicWord>> Count(string row)
        {
            var words = Regex.Split(row.ToLower(), @"\W+").ToList();

            var spellCheckResult = SpellCheck(words);
            if (!spellCheckResult.IsSuccess)
                return Result.Fail<List<GraphicWord>>(spellCheckResult.Error);
            
            var countedWords = CountWords(spellCheckResult.Value);
            SetFontSize(countedWords.Values);
            
            var stopWordsResult = GetStopWords();
            if (!stopWordsResult.IsSuccess)
                return Result.Fail<List<GraphicWord>>(stopWordsResult.Error);

            var stopWords = stopWordsResult.Value;

            return countedWords
                .Values
                .Where(w => w.Rate > 1 && !stopWords.Contains(w.Value))
                .OrderByDescending(w => w.Rate).ToList();
        }

        private Dictionary<string, GraphicWord> CountWords(List<string> words)
        {
            var countedWords = new Dictionary<string, GraphicWord>();
            foreach (var word in words)
            {
                if (!countedWords.ContainsKey(word))
                    countedWords[word] = new GraphicWord(word);
                else
                    countedWords[word].Rate++;
            }

            return countedWords;
        }

        private void SetFontSize(IEnumerable<GraphicWord> words)
        {
            foreach (var graphicWord in words)
            {
                SetFontSize(graphicWord);
            }
        }

        private void SetFontSize(GraphicWord word)
        {
            word.Font = new Font(Font.Name, word.Rate + Font.Size);
        }
    }
}
