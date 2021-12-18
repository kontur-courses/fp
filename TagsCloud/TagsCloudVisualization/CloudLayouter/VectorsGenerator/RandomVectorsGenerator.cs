using System;
using System.Drawing;
using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudVisualization.CloudLayouter.VectorsGenerator
{
    public class RandomVectorsGenerator : IVectorsGenerator
    {
        private readonly Random _random;
        private readonly PositiveSize _sizeRange;

        private RandomVectorsGenerator(Random random, PositiveSize sizeRange)
        {
            _random = random;
            _sizeRange = sizeRange;
        }

        public static Result<RandomVectorsGenerator> Create(Random random, PositiveSize sizeRange)
        {
            return Result.Ok()
                .ValidateNonNull(random, nameof(random))
                .ToValue(new RandomVectorsGenerator(random, sizeRange));
        }

        public Point GetNextVector() =>
            new(GenerateFromSegment(-_sizeRange.Width, _sizeRange.Width),
                GenerateFromSegment(-_sizeRange.Height, _sizeRange.Height));

        private int GenerateFromSegment(int min, int max) => _random.Next(max - min) + min;
    }
}