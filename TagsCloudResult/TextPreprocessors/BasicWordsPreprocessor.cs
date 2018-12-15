using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return ProcessWords(Result.Of(GetHunspellInstance)
                .Then(hunspell => PrepareWords(words, hunspell.Value))
                .RefineError("Не удалось подготовить выборку"));
        }

        private Result<Hunspell> GetHunspellInstance()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "HunspellDicts", "Russian");
            var affFile = Path.Combine(dir, "ru.aff");
            var dictFile = Path.Combine(dir, "ru.dic");

            return Result.Of(() => new Hunspell(affFile, dictFile))
                .RefineError("Не удалось создать экземпляр Hunspell");
        }

        private Result<List<string>> PrepareWords(IEnumerable<string> words, Hunspell hunspell)
        {
            var preprocessedWords = new List<string>();

            using (hunspell)
            {
                foreach (var word in words)
                {
                    var lemma = hunspell.Stem(word).FirstOrDefault();

                    if (lemma != null && WordMeetsAllRequirements(lemma))
                        preprocessedWords.Add(lemma.ToLower());
                }
            }

            return preprocessedWords;
        }

        private bool WordMeetsAllRequirements(string lemma)
        {
            return wordFilters.All(e => e.Filter(lemma));
        }

        private Result<IReadOnlyDictionary<string, int>> ProcessWords(Result<List<string>> preprocessWords)
        {
            return Result.Of(() =>
            {
                var result = new Dictionary<string, int>();

                foreach (var word in preprocessWords.Value)
                {
                    result.TryGetValue(word, out var count);
                    result[word] = count + 1;
                }

                return (IReadOnlyDictionary<string, int>)result;
            });
        }
    }
}
