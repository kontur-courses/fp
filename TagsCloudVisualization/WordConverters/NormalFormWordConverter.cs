using System.Collections.Generic;
using System.Linq;
using NHunspell;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.WordConverters
{
    public class NormalFormWordConverter : IWordConverter
    {
        private readonly string affPath;
        private readonly string dicPath;

        public NormalFormWordConverter(string affPath, string dicPath)
        {
            this.affPath = affPath;
            this.dicPath = dicPath;
        }

        public Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words)
        {
            var normalFormWords = new List<string>();
            using (var hunspell = new Hunspell(affPath, dicPath))
            {
                foreach (var word in words)
                {
                    var normalFormsResult = Result.Of(() => hunspell.Stem(word));
                    if (!normalFormsResult.IsSuccess)
                        return Result.Fail<IEnumerable<string>>("Failed to convert to normal form");
                    normalFormWords.Add(normalFormsResult.GetValueOrThrow().Count > 0
                        ? normalFormsResult.GetValueOrThrow().First()
                        : word);
                }
            }

            return normalFormWords.AsEnumerable().AsResult();
        }
    }
}