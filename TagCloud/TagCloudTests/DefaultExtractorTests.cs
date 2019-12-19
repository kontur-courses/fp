using FluentAssertions;
using NUnit.Framework;
using System;
using TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    public class DefaultExtractorTests
    {
        private DefaultExtractor defaultExtractor;

        [SetUp]
        public void BaseSetUp()
        {
            defaultExtractor = new DefaultExtractor();
        }

        [Test]
        public void ExtractWordTokenShould_ThrowArgumentNullException_OnNullText()
        {
            Action action = () => defaultExtractor.ExtractWords(null).GetValueOrThrow();
            action.Should().Throw<InvalidOperationException>().WithMessage("Error occured: Text cannot be null");
        }

        [TestCase("", ExpectedResult = 0)]
        [TestCase("\r\n", ExpectedResult = 0)]
        [TestCase("\r\n\r\n\r\n", ExpectedResult = 0)]
        [TestCase("Foo", ExpectedResult = 1)]
        [TestCase("Foo\r\n", ExpectedResult = 1)]
        [TestCase("Foo\r\nFoo", ExpectedResult = 2)]
        public int ExtractWordTokenShould_ReturnCorrectNumberOfWords(string text)
        {
            return defaultExtractor.ExtractWords(text).GetValueOrThrow().Length;
        }
    }
}
