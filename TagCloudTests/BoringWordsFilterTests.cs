using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Analyzers;
using TagCloud.Provider;

namespace TagCloudTests
{
    public class BoringWordsFilterTests
    {
        private BoringWordsFilter filter;
        private IWordProvider wordProvider;

        [SetUp]
        public void SetUp()
        {
            wordProvider = new WordProvider();
            filter = new BoringWordsFilter(wordProvider);
        }

        [Test]
        public void Analyze_ShouldSkipBoringWords()
        {
            var text = new[] { "I", "met", "you", "a", "long", "time", "ago" };
            var boringWords = new HashSet<string> { "I", "you", "a", "ago" };
            wordProvider.AddWords(boringWords);

            var analyzedWords = filter.Analyze(text);

            analyzedWords.Should().BeEquivalentTo("met", "long", "time");
        }
    }
}
