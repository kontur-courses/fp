using ResultOf;
using System.Drawing;
using TagsCloudContainer.TagCloudBuilder;
using TagsCloudVisualization;
using FluentAssertions;

namespace ResutlTests
{
    public class PointsProviderTests
    {
        private const int maxPointsCount = 10000000;

        [Test]
        [TestCaseSource(nameof(PointProviders))]
        public void Points_ShouldReturnCorrectNumberOfPoints(IPointsProvider provider)
        {
            // Arrange
            var center = new Point(10, 10);
            provider.Initialize(center);

            // Act
            var sut = provider.Points().Skip(maxPointsCount-2).First();

            // Assert
            sut.IsSuccess.Should().BeTrue();
        }


        [Test]
        [TestCaseSource(nameof(PointProviders))]
        public void Points_ShouldReturnFailWhenMaxPointsCountReached(IPointsProvider provider)
        {
            // Arrange
            var center = new Point(10, 10);
            provider.Initialize(center);

            // Act
            var result = provider.Points().Last();

            // Assert
            result.Should().BeOfType<Result<Point>>();
            ((Result<Point>)result).IsSuccess.Should().BeFalse();
        }

        public static IEnumerable<IPointsProvider> PointProviders()
        {
            var testCase1 = new NormalPointsProvider();
            var testCase2 = new RandomPointsProvider();
            var testCase3 = new SpiralPointsProvider();
            return new List<IPointsProvider>() { testCase1, testCase2, testCase3 };
        }
    }
}
