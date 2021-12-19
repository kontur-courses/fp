using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainerTests.ParserTests
{
    internal abstract class ParserTests
    {
        private readonly string textsFolder = Path.GetFullPath(@"..\..\..\texts");
        protected IParser parser;
        protected string format;

        [Test]
        public void Should_Throw_OnNonExistingFile()
        {
            var path = Path.Combine(textsFolder, $"amogus.{format}");

            parser.Parse(path).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Should_ParseCorrectly()
        {
            var path = Path.Combine(textsFolder, $"parserTest.{format}");

            var result = parser.Parse(path);
            var expected = new[] { "this", "Is", " parser", "test " };

            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
