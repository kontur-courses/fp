﻿using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NHunspell;
using NUnit.Framework;
using TagCloud.WordsFilter;

namespace TagCloudTest
{
    [TestFixture]
    public class WordsFilterTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var dictionaryAff = Path.GetFullPath("../../../dictionaries/en.aff");
            var dictionaryDic = Path.GetFullPath("../../../dictionaries/en.dic");
            wordsFilter = new WordsFilter().RemovePrepositions().Normalize(new Hunspell(dictionaryAff, dictionaryDic));
        }

        private IWordsFilter wordsFilter;

        [Test]
        public void Prepositions_ShouldBeRemoved()
        {
            wordsFilter.Apply(new List<string> {"123", "in", "of", "word", "anotherword"})
                .Should().BeEquivalentTo(new List<string> {"123", "word", "anotherword"});
        }

        [Test]
        public void Words_ShouldBeNormalized()
        {
            wordsFilter.Apply(new List<string> {"123", "in", "of", "contains", "words", "anoTHerWord"})
                .Should().BeEquivalentTo(new List<string> {"123", "contain", "word", "anotherword"});
        }
    }
}