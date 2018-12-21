using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult;

namespace TagsCloudContainer.ColorAlgorithm
{
    public class RandomColorAlgorithm : IColorAlgorithm
    {
        private readonly Random rnd = new Random();

        public Result<Color> GetColor(Dictionary<string, int> words = null, string word = "")
        {
            return Result.Ok(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
        }
    }
}
