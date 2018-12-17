using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.WordsProcessing;

namespace TagsCloudVisualization_Tests.WordProcessing
{
    [TestFixture]
    public class FrequencyWeighter_Should
    {
        private IEnumerable<string> words;
        private FrequencyWeighter weighter;

        
        [SetUp]
        public void SetUp()
        {
            weighter = new FrequencyWeighter();
            words = new[] {"a", "b"};
        }

        [Test]
        public void WeightWordsCorrectly_WhenRepeatingWords()
        {
            words = new[] { "a", "b", "a" };
            var expected = new[] {new WeightedWord("a", 2), new WeightedWord("b", 1)};
            weighter.WeightWords(words).GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WeightWordsCorrectly_WhenNoRepeatingWords()
        {
           var expected = new[] {new WeightedWord("a", 1), new WeightedWord("b", 1)};
            weighter.WeightWords(words).GetValueOrThrow().Should().BeEquivalentTo(expected);
        }


        [Test]
        public void WeightWordsCorrectly_ReturnOrderedByDescendingAccordingWeight()
        {
            weighter.WeightWords(words).GetValueOrThrow().Should().BeInDescendingOrder(c => c.Weight);
        }

        [Test]
        public void WeightWords_WhenProviderEmpty()
        {
            words = Array.Empty<string>();
            weighter.WeightWords(words).GetValueOrThrow().Should().BeEmpty();
        }

    }
}
