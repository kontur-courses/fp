﻿using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization
{
    public class PointGeneratorDetector : IPointGeneratorDetector
    {
        private readonly IEnumerable<IPointGenerator> pointGenerators;

        public PointGeneratorDetector(IEnumerable<IPointGenerator> pointGenerators)
        {
            this.pointGenerators = pointGenerators;
        }

        public IPointGenerator GetPointGenerator(string name)
        {
            return pointGenerators.FirstOrDefault(pointGenerator => name == pointGenerator.GetType().Name.ToLower());
        }

        public Result<string> ValidatePointGeneratorIsSupported(string pointGenerator)
        {
            return Result.Validate(pointGenerator, n => pointGenerators.Any(g => g.GetType().Name.ToLower() == n),
                $"Invalid point generator name '{pointGenerator}'");
        }
    }
}