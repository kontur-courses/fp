using NHunspell;
using ResultOf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordPreprocessing
    {
        public Result<IEnumerable<string>> Words { get; private set; }

        public WordPreprocessing(Result<string[]> words)
        {
            Words = words.Then(w => w as IEnumerable<string>);
        }

        public WordPreprocessing ToLower()
        {
            Words = Words.Then(w1 => w1.Select(w => w.ToLower()));
            return this;
        }

        public WordPreprocessing IgnoreInvalidWords()
        {
            Words = Words.Then(words =>
            {
                using (Hunspell hunspell = new Hunspell("ru_RU.aff", "ru_RU.dic"))
                {
                    var newWords = new List<string>();
                    foreach (var word in words)
                    {
                        var stem = hunspell.Stem(word);
                        if (stem.Count > 0)
                            newWords.Add(stem[0]);
                    }

                    return newWords as IEnumerable<string>;
                }
            }).RefineError("Hunspell didn't find dictionaries");
            return this;
        }
        
        public WordPreprocessing Exclude(Result<HashSet<string>> wordsToExclude)
        {
            if (wordsToExclude.IsSuccess)
                Words = Words.Then(words => words.Where(w => !wordsToExclude.Value.Contains(w)));
            return this;
        }

        public WordPreprocessing CustomPreprocessingSelect(Func<string, string> func)
        {
            Words = Words.Then(words => words.Select(func));
            return this;
        }

        public WordPreprocessing CustomPreprocessingWhere(Func<string, bool> func)
        {
            Words = Words.Then(words => words.Where(func));
            return this;
        }
    }
}
