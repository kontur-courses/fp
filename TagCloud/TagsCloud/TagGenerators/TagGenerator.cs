using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.TagGenerators
{
    public class TagGenerator : ITagGenerator
    {
        private readonly IColorScheme colorGenerator;
        private readonly IFontSettingsGenerator fontGenerator;

        public TagGenerator(IFontSettingsGenerator fontGenerator, IColorScheme colorGenerator)
        {
            this.fontGenerator = fontGenerator;
            this.colorGenerator = colorGenerator;
        }

        public Result<IEnumerable<Tag>> GenerateTags(Dictionary<string, int> wordsStatistics)
        {
            if (wordsStatistics == null)
                return Result.Fail<IEnumerable<Tag>>($"{nameof(wordsStatistics)} cannot be null");
            var wordsOrderedByFrequency = wordsStatistics.Keys.OrderBy(word => wordsStatistics[word]).ToList();
            var countWord = wordsOrderedByFrequency.Count;
            var result = new List<Tag>();
            var errors = new List<string>();
            for (var currentPositionWord = 0; currentPositionWord < countWord; currentPositionWord++)
            {
                var word = wordsOrderedByFrequency[currentPositionWord];
                var fontSettings = fontGenerator
                    .GetFontSizeForCurrentWord((word, wordsStatistics[word]), currentPositionWord, countWord)
                    .RefineError("Font settings cannot be obtained")
                    .OnFail(error => errors.Add(error));
                var color = colorGenerator
                    .GetColorForCurrentWord((word, wordsStatistics[word]), currentPositionWord, countWord)
                    .RefineError("Color cannot be obtained")
                    .OnFail(error => errors.Add(error));
                if (!fontSettings.IsSuccess || !color.IsSuccess) continue;
                result.Add(new Tag(fontSettings.Value, color.Value, word));
            }

            return errors.Count != 0
                ? Result.Fail<IEnumerable<Tag>>(string.Join(Environment.NewLine, errors))
                : result.AsEnumerable().AsResult();
        }
    }
}