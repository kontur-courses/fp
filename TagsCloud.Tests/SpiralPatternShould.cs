using System.Drawing;
using FluentAssertions;
using TagCloud.FigurePatterns.Implementation;
using TagsCloud.Tests;

namespace TagCloud.Should
{
    [TestFixture]
    public class SpiralPatternShould
    {
        private SpiralPatterPointProvider spiralPatterPointProvider;

        [SetUp]
        public void SetUp()
        {
            spiralPatterPointProvider = new SpiralPatterPointProvider(Point.Empty, 1);
        }
        
        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectStepCount))]
        [Parallelizable(scope: ParallelScope.All)] 
        public void Ctor_IncorrectStep_ArgumentException(int steps)
        {
            // ReSharper disable once ObjectCreationAsStatement
            var createSpiralPattern = (Action) (() => new SpiralPatterPointProvider(Point.Empty, steps));
            createSpiralPattern.Should().Throw<ArgumentException>();
        }
    }
}