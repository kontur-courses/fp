using System;
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

        public Result<IEnumerable<Tag>> GenerateTags(IEnumerable<(string word, int frequency)> wordsStatistics)
        {
            if (wordsStatistics == null)
                return Result.Fail<IEnumerable<Tag>>($"{nameof(wordsStatistics)} cannot be null");
            var currentPosition = 0;
            var orderedStatistics = wordsStatistics.OrderBy(wordsStatistic => wordsStatistic.frequency).ToList();
            var countWord = orderedStatistics.Count;
            var result = new List<Tag>();
            var errors = new List<string>();
            foreach(var wordStatistics in orderedStatistics)
            {
                var fontSettings = fontGenerator.GetFontSizeForCurrentWord(wordStatistics, currentPosition, countWord)
                    .RefineError("Font settings cannot be obtained")
                    .OnFail((error) => errors.Add(error));
                var color = colorGenerator.GetColorForCurrentWord(wordStatistics, currentPosition, countWord)
                    .RefineError("Color cannot be obtained")
                    .OnFail((error) => errors.Add(error));
                if (!fontSettings.IsSuccess || !color.IsSuccess) continue;
                result.Add(new Tag(fontSettings.Value, color.Value, wordStatistics.word));
                currentPosition++;
            }
            return errors.Count != 0 
                ? Result.Fail<IEnumerable<Tag>>(string.Join(Environment.NewLine, errors)) 
                : result.AsEnumerable().AsResult();
        }
    }
}
