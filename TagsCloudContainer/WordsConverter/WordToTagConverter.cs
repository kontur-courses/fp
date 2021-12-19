using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.CloudLayouter;

namespace TagsCloudContainer.WordsConverter
{
    public class WordToTagConverter : IWordConverter
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly IFontCreator fontCreator;

        public WordToTagConverter(ICloudLayouter cloudLayouter, IFontCreator fontCreator)
        {
            this.cloudLayouter = cloudLayouter;
            this.fontCreator = fontCreator;
        }

        public Result<List<Tag>> ConvertWords(List<string> words)
        {
            var wordsFrequency = GetWordsFrequency(words).OrderByDescending(pair => pair.Value).ToList();
            var maxFrequency = wordsFrequency.First().Value;
            var tags = new List<Tag>();
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);

            foreach (var (word, frequency) in wordsFrequency)
            {
                var tagFont = GetFont(frequency, maxFrequency);
                if (!tagFont.IsSuccess)
                    return Result.Fail<List<Tag>>(tagFont.Error);
                
                var tagRectangleSize = Size.Ceiling(graphics.MeasureString(word, tagFont.Value));
                var tagRectangle = cloudLayouter.PutNextRectangle(tagRectangleSize);

                if (!tagRectangle.IsSuccess)
                    return Result.Fail<List<Tag>>(tagRectangle.Error);
                
                tags.Add(new Tag(word, tagRectangle.Value, tagFont.Value));
            }
            
            return Result.Ok(tags);
        }

        private Result<Font> GetFont(int wordFrequency, int maxFrequency)
        {
            var fontName = fontCreator.GetFontName();
            if (!fontName.IsSuccess)
                return Result.Fail<Font>(fontName.Error);
            
            var fontSize = fontCreator.GetFontSize(wordFrequency, maxFrequency);
            return Result.Ok(new Font(fontName.Value, fontSize));
        }

        private static Dictionary<string, int> GetWordsFrequency(IEnumerable<string> words)
        {
            return words.GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }
    }
}