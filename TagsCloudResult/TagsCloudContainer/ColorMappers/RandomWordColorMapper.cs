using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.ColorMappers
{
    public class RandomWordColorMapper : IWordColorMapper
    {
        public WordColorMapperType Type => WordColorMapperType.Random;

        private readonly Random random;

        public RandomWordColorMapper(Random random)
        {
            this.random = random;
        }

        public Result<Dictionary<WordLayout, Color>> GetColorMap(CloudLayout layout)
        {
            return layout.WordLayouts.ToDictionary(
                wordLayout => wordLayout,
                _ => Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
        }
    }
}