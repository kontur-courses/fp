using System;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.ColorSchemes
{
    public class RandomColorScheme : IColorScheme
    {
        public Result<Color> GetColorForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency,
            int countWords)
        {
            if (countWords == 0)
                return Result.Fail<Color>("The count of words cannot be zero");
            var rnd = new Random();
            var alpha = (int) (255 * ((float) (countWords - positionByFrequency + 1) / countWords));
            alpha = Math.Min(alpha, 255);
            return Color.FromArgb(alpha, rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255)).AsResult();
        }
    }
}