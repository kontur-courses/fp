using System.Collections.Generic;
using System.Linq;
using NHunspell;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class BaseFormConverter : IWordsChanger
    {
        private readonly string affFileName;
        private readonly string dicFileName;

        public BaseFormConverter(string affFileName, string dicFileName)
        {
            this.affFileName = affFileName;
            this.dicFileName = dicFileName;
        }

        public Result<IEnumerable<string>> ChangeWords(IEnumerable<string> words)
        {
            return Result.Of(() => new Hunspell(affFileName, dicFileName))
                .RefineError("Failed opening Hunspell Dictionaries")
                .Then(hunspell => GetWordsBase(hunspell, words).ToList().AsEnumerable());
        }

        private IEnumerable<string> GetWordsBase(Hunspell hunspell, IEnumerable<string> words)
        {
            using (hunspell)
            {
                foreach (var word in words)
                {
                    var stems = hunspell.Stem(word);
                    yield return stems.Count != 0 ? stems.First() : word;
                }
            }
        }
    }
}
