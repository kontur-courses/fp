using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.Tests
{
    public class WordFilter_Should
    {
        private IEnumerable<string> words;
        [SetUp]
        public void SetUp()
        {
            words = new[] {"сл", "сло", "слово", "word"};
        }


        [Test]
        public void FilterWordsByLengthBiggerThan2_ByDefault()
        {
            var wf = new WordFilter();
            var filtered = wf.Filter(words);
            filtered.GetValueOrThrow().Should().BeEquivalentTo("сло", "слово", "word");
        }

        [Test]
        public void FilterWords_WithConfiguration()
        {
            var wf = new WordFilter(x => !x.Contains("с"));
            var filtered = wf.Filter(words);
            filtered.GetValueOrThrow().Should().BeEquivalentTo("word");
        }

    }
}