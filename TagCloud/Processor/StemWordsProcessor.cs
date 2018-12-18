using System.Collections.Generic;
using NHunspell;
using TagCloud.Data;

namespace TagCloud.Processor
{
    public class StemWordsProcessor : IWordsProcessor
    {
        private const string AffFile = @"..\..\Dictionaries\ru.aff";
        private const string DictFile = @"..\..\Dictionaries\ru.dic";

        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return Result.Of(() => StemWords(words),
                $"Hunspell dictionaries not found. Check existence of {AffFile} and {DictFile}");
        }

        private static IEnumerable<string> StemWords(IEnumerable<string> words)
        {
            using (var hunspell = new Hunspell(AffFile, DictFile))
            {
                foreach (var word in words)
                {
                    var stems = hunspell.Stem(word);
                    yield return stems.Count > 0 ? stems[0] : word;
                }
            }
        }
    }
}