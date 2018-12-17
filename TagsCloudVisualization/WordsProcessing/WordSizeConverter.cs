using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ResultOf;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualization.WordsProcessing
{
    public class WordSizeConverter : ISizeConverter
    {
        private readonly FontSettings fontSettings;
        
        public WordSizeConverter(FontSettings fontSettings)
        {
            this.fontSettings = fontSettings;
        }
        
        public Result<IEnumerable<SizedWord>> Convert(IEnumerable<WeightedWord> weightedWords)
        {
            var maxWeight = weightedWords.Max().Weight;
            var minWeight = weightedWords.Min().Weight;
            var value = new List<SizedWord>();
            foreach (var word in weightedWords)
            {
                var fontResult = fontSettings.GetFont(word.Weight, minWeight, maxWeight);
                var sizeResult = fontResult.Then(font => GetSize(word.Word, font));
                if (!fontResult.IsSuccess || !sizeResult.IsSuccess)
                    return Result.Fail<IEnumerable<SizedWord>>(sizeResult.Error); 
                value.Add(new SizedWord(word.Word, fontResult.Value, sizeResult.Value));
            }
            return Result.Ok(value.AsEnumerable());
        }

        private Result<Size> GetSize(string text, Font font)
        {
            return text.Length == 0 
                ? Result.Fail<Size>("Text length should be grater than zero") 
                : Result.Of(() => TextRenderer.MeasureText(text, font));
        }
    }
}