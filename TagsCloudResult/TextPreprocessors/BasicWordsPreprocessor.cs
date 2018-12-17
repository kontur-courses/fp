using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;
using TagsCloudResult.TextPreprocessors.Filters;

namespace TagsCloudResult.TextPreprocessors
{
    public class BasicWordsPreprocessor : IWordsPreprocessor
    {
        private readonly IEnumerable<IWordFilter> wordFilters;

        public BasicWordsPreprocessor(IEnumerable<IWordFilter> wordFilters)
        {
            this.wordFilters = wordFilters;
        }
        public Result<IReadOnlyDictionary<string, int>> PreprocessWords(IEnumerable<string> words)
        {
            return ProcessWords(PrepareWords(words))
                .RefineError("Не удалось подготовить выборку");
        }

        private Result<List<string>> PrepareWords(IEnumerable<string> words)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "HunspellDicts", "Russian");
            var affFile = Path.Combine(dir, "ru.aff");
            var dictFile = Path.Combine(dir, "ru.dic");

            var preprocessedWords = new List<string>();

            using (var hunspell = new Hunspell(affFile, dictFile))
            {
                foreach (var word in words)
                {
                    var lemma = hunspell.Stem(word).FirstOrDefault();

                    if (lemma != null && CheckIfWordMeetsAllRequirements(lemma))
                        preprocessedWords.Add(lemma.ToLower());
                }
            }

            return preprocessedWords;
        }

        private bool CheckIfWordMeetsAllRequirements(string lemma)
        {
            return wordFilters.All(e => e.Filter(lemma));
        }

        private Result<IReadOnlyDictionary<string, int>> ProcessWords(Result<List<string>> preprocessWords)
            => preprocessWords.Value
                .GroupBy(e => e)
                .ToDictionary(e => e.Key, e => e.Count());
    }
}
