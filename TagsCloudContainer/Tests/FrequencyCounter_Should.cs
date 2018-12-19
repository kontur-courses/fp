using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.Tests
{
    public class FrequencyCounter_Should
    {
        private readonly string[] words =
        {
            "something",
            "foo",
            "shpora",
            "something",
            "kontur",
            "shpora",
            "bar",
            "c-diez",
            "foo",
            "shpora",
            "foo",
            "a",
            "in",
            "o",
            "I",
            "of",
            "ye"
        };

        [Test]
        public void OrderWordsByFrequency_DyDescending()
        {
            var frequencyCounter = new FrequencyCounter();

            var result = frequencyCounter.CountWordFrequencies(words).GetValueOrThrow().Select(info => info.Frequency);

            var previous = int.MaxValue;
            foreach (var frequency in result)
            {
                frequency.Should().BeLessOrEqualTo(previous);
                previous = frequency;
            }
        }

        [Test]
        public void CountWordsFrequency()
        {
            var frequencyCounter = new FrequencyCounter();

            var result = frequencyCounter.CountWordFrequencies(words).GetValueOrThrow();

            foreach (var info in result)
            {
                var frequency = words.Count(word => word == info.Word);
                info.Frequency.Should().Be(frequency);
            }
        }

        [Test]
        public void BeNotSucceed_WhenWordsAreNull()
        {
            var frequencyCounter = new FrequencyCounter();

            var resultAction = frequencyCounter.CountWordFrequencies(null);

            resultAction.IsSuccess.Should().BeFalse();
        }
    }
}