using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private Result<List<string>> CheckIfHunspellDictsPresent(string affFile, string dictFile)
        {
            var message = new StringBuilder("Необходимые для работы Hunspell файлы отсутствуют: ");

            if(!CheckIfDictExists(affFile, message) | !CheckIfDictExists(dictFile, message))
                return Result.Fail<List<string>>(message.ToString());

            return Result.Ok<List<string>>(null);
        }

        private static bool CheckIfDictExists(string dictFileName, StringBuilder message)
        {
            if (File.Exists(dictFileName)) return true;
            message.Append(dictFileName);
            return false;
        }

        private Result<List<string>> PrepareWords(IEnumerable<string> words)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "HunspellDicts", "Russian");
            var affFile = Path.Combine(dir, "ru.aff");
            var dictFile = Path.Combine(dir, "ru.dic");

            var hunspellDictsPresent = CheckIfHunspellDictsPresent(affFile, dictFile);
            if (!hunspellDictsPresent.IsSuccess)
                return hunspellDictsPresent;

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
