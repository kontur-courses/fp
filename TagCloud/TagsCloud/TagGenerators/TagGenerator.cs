using System.Collections.Generic;
using TagsCloud.Interfaces;
using System.Linq;
using TagsCloud.ErrorHandling;

namespace TagsCloud.TagGenerators
{
    public class TagGenerator : ITagGenerator
    {
        private readonly IFontSettingsGenerator fontGenerator;
        private readonly IColorScheme colorGenerator;

        public TagGenerator(IFontSettingsGenerator fontGenerator, IColorScheme colorGenerator)
        {
            this.fontGenerator = fontGenerator;
            this.colorGenerator = colorGenerator;
        }

        public Result<IEnumerable<Tag>> GenerateTag(IEnumerable<(string word, int frequency)> wordStatistics)
        {
            var currentPosition = 0;
            var countWord = wordStatistics.Count();
            wordStatistics = wordStatistics.OrderBy(wordsStatistic => wordsStatistic.frequency);
            var result = new List<Tag>();
            foreach(var wordStatistic in wordStatistics)
            {
                var fontSettings = fontGenerator.GetFontSizeForCurrentWord(wordStatistic, currentPosition, countWord);
                var color = colorGenerator.GetColorForCurrentWord(wordStatistic, currentPosition, countWord);
                if (!fontSettings.IsSuccess)
                {
                    fontSettings.RefineError("Font settings cannot be obtained");
                    return Result.Fail<IEnumerable<Tag>>(fontSettings.Error);
                }
                if (!color.IsSuccess)
                {
                    color.RefineError("Collor cannot be obtained");
                    return Result.Fail<IEnumerable<Tag>>(color.Error);
                }
                result.Add(new Tag(fontSettings.Value, color.Value, wordStatistic.word));
            }
            return (result.AsEnumerable()).AsResult();
        }
    }
}
