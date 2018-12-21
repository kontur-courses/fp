using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;

namespace TagsCloudResult
{
    internal class WordsCustomizer
    {
        private List<string> wordsToIgnore;
        private Func<string, bool> shouldIgnoreWord;
        private readonly string enUsAffPath;
        private readonly string enUsDicPath;
 
        public WordsCustomizer(List<string> wordsToIgnore = null, Func<string, bool> shouldIgnoreWord = null)
        {
            this.wordsToIgnore = wordsToIgnore ?? StandardWordsToIgnorePack();
            this.shouldIgnoreWord = shouldIgnoreWord ?? (x => false);

            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            enUsAffPath = Path.Combine(dirPath, "..", "..", "EnglishDictionaries", "en_us.aff");
            enUsDicPath = Path.Combine(dirPath, "..", "..", "EnglishDictionaries", "en_us.dic");
        }

        public Result<string> CustomizeWord(string word)
        {
            if (word == null)
                new Result<string>("word to customize shouldn't be null");

            return IgnoreWord(word) ? new Result<string>("word should be ignored") : ToBaseForm(word.ToLower());
        }

        public bool IgnoreWord(string word)
        {
            return wordsToIgnore.Contains(word.ToLower()) || shouldIgnoreWord(word);
        }

        private Result<string> ToBaseForm(string word)
        {
            string result;
            try
            {
                using (var hunspell = new Hunspell(enUsAffPath, enUsDicPath))
                {
                    result = hunspell.Stem(word).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return new Result<string>(e.Message);
            }

            return string.IsNullOrEmpty(result) ? 
                new Result<string>(null, word) : new Result<string>(null, result);
        }

        private static List<string> StandardWordsToIgnorePack()
        {
            return Prepositions();
        }

        public static List<string> Prepositions()
        {
            return new List<string>
            {
                "to", "into", "up", "down", "through", "out", "across", "along", "at", "by", "on", "in", "above",
                "under", "among", "between", "behind", "in front of", "next to",
                "about", "after", "at", "during", "for", "in", "on", "till", "within"
            };
        }

        public void SetIgnoreFunc(Func<string, bool> ignoreFunc)
        {
            shouldIgnoreWord = ignoreFunc;
        }

        public void SetWordsToIgnore(IEnumerable<string> newWordsToIgnore)
        {
            wordsToIgnore = newWordsToIgnore.ToList();
        }

        public void AddWordsToIgnore(IEnumerable<string> newWordsToIgnore)
        {
            wordsToIgnore.AddRange(newWordsToIgnore);
        }

        public List<string> GetWordsToIgnore()
        {
            return wordsToIgnore;
        }
    }
}