using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Words;

namespace TagsCloud.Tests
{
    public class LowerWordCollectionTests
    {
        private readonly List<string> expectedWords = new List<string>
        {
            "neural",
            "networks",
            "are",
            "not",
            "the",
            "only",
            "machine",
            "learning",
            "frameworks"
        };

        [Test]
        public void GetWords_DifferentCase_AllWordsLowerCase()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data/DifferentCases.txt");
            var words = new LowerWords(new WordsFromFile(path)).ToLower();
            words.Value.Should().BeEquivalentTo(expectedWords);
        }
    }
}

