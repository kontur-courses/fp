using FluentAssertions;
using ResultOf;
using System.Drawing;
using TagsCloudContainer.TagCloudBuilder;
using TagsCloudVisualization;

namespace ResultTests
{
    public class PointsProviderTests
    {
        private const int maxPointsCount = 10000000;

        [Test]
        [TestCaseSource(nameof(PointProviders))]
        public void Points_ShouldReturnCorrectNumberOfPoints(IPointsProvider provider)
        {
            var sut = provider.Points().Skip(maxPointsCount - 2).First();

            sut.IsSuccess.Should().BeTrue();
        }


        [Test]
        [TestCaseSource(nameof(PointProviders))]
        public void Points_ShouldReturnFailWhenMaxPointsCountReached(IPointsProvider provider)
        {
            var result = provider.Points().Last();

            result.Should().BeOfType<Result<Point>>();
            result.IsSuccess.Should().BeFalse();
        }

        public static IEnumerable<IPointsProvider> PointProviders()
        {
            var testCase1 = new NormalPointsProvider(new Point(300, 300));
            var testCase2 = new RandomPointsProvider(new Point(300, 300));
            var testCase3 = new SpiralPointsProvider(new Point(300, 300));
            return new List<IPointsProvider>() { testCase1, testCase2, testCase3 };
        }
    }
}