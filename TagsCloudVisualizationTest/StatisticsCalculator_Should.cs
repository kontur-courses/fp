using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Utils;


namespace TagsCloudVisualizationTest
{
    [TestFixture]
    class StatisticsCalculator_Should
    {
        private readonly StatisticsCalculator statisticsCalculator = new StatisticsCalculator();

        [Test]
        public void ReturnEmptyStatistics_WhenNoWords()
        {
            var statisticsResult = statisticsCalculator.CalculateStatistics(new string[] { });

            statisticsResult.IsSuccess.Should().BeTrue();
            statisticsResult.GetValueOrThrow().OrderedWordsStatistics.Should().BeEmpty();
            statisticsResult.GetValueOrThrow().AllWordsCount.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public void ReturnRightAllWordsCount_WhenAllWordsUnique()
        {
            var statisticsResult = statisticsCalculator.CalculateStatistics(new [] { "car", "phone" });

            statisticsResult.IsSuccess.Should().BeTrue();
            statisticsResult.GetValueOrThrow().AllWordsCount.Should().Be(2);
        }

        [Test]
        public void ReturnRightAllWordsCount_WhenSomeWordsAreTheSame()
        {
            var statisticsResult = statisticsCalculator.CalculateStatistics(new[] { "car", "phone", "car" });

            statisticsResult.IsSuccess.Should().BeTrue();
            statisticsResult.GetValueOrThrow().AllWordsCount.Should().Be(3);
        }

        [Test]
        public void CountWordsCorrectly()
        {
            var statisticsResult = statisticsCalculator.CalculateStatistics(new[] { "car", "phone", "car" });

            statisticsResult.IsSuccess.Should().BeTrue();
            statisticsResult.GetValueOrThrow().OrderedWordsStatistics.Should().BeEquivalentTo(
                new WordStatistics("car", 2), new WordStatistics("phone", 1));

        }

        [Test]
        public void OrderWordStatistics()
        {
            var statisticsResult = statisticsCalculator.CalculateStatistics(new[] { "car", "phone", "car" });

            statisticsResult.IsSuccess.Should().BeTrue();
            statisticsResult.GetValueOrThrow().OrderedWordsStatistics.Select(ws => ws.Count).Should().BeInDescendingOrder();
        }
    }}
