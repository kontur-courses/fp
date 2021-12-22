#region

using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

#endregion

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class FrequencyCounterTests
    {
        private readonly FrequencyCounter frequencyCounter = new();

        [Test]
        public void FrequencyCounter_ShouldCountFrequency_WhenInputIsCorrect()
        {
            var words = new List<string>
            {
                "a", "b", "c", "c", "a", "a"
            };

            var actual = frequencyCounter.GetFrequencyDictionary(words);

            actual.GetValueOrThrow().Should().Contain(new KeyValuePair<string, int>("a", 3));
            actual.GetValueOrThrow().Should().Contain(new KeyValuePair<string, int>("b", 1));
            actual.GetValueOrThrow().Should().Contain(new KeyValuePair<string, int>("c", 2));
        }

        [Test]
        public void FrequencyCounter_ShouldReturnEmptyDictionary_WhenInputIsEmpty()
        {
            var actual = frequencyCounter.GetFrequencyDictionary(Array.Empty<string>());

            actual.GetValueOrThrow().Should().BeEmpty();
        }

        [Test]
        public void FrequencyCounter_ShouldReturnUnsuccessfulResult_WhenInputIsNull()
        {
            var actual = frequencyCounter.GetFrequencyDictionary(null);

            actual.IsSuccess.Should().BeFalse();
        }
    }
}