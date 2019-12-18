using System;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.ColorSchemes
{
    internal class RedGreenBlueScheme : IColorScheme
    {
        public Result<Color> GetColorForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency,
            int countWords)
        {
            if (countWords == 0)
                return Result.Fail<Color>("The count of words cannot be zero");
            var alpha = (int) (255 * ((float) (countWords - positionByFrequency + 1) / countWords));
            var position = (float) positionByFrequency / countWords;
            var color = position < 0.1 ? Color.Red : position < 0.40 ? Color.Green : Color.Blue;
            alpha = Math.Min(255, alpha);
            return Color.FromArgb(alpha, color).AsResult();
        }
    }
}