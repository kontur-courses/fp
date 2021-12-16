using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Results;

namespace TagsCloud.Tests
{
    public class EnumParserTests
    {
        private EnumParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new EnumParser();
        }

        [TestCase("A", SpeechPart.A)]
        [TestCase("ADV", SpeechPart.ADV)]
        public void Parse_ReturnCorrectValue(string value, SpeechPart expected)
        {
            parser.Parse<SpeechPart>(value)
                .Should().BeEquivalentTo(Result.Ok(expected));
        }

        [TestCase("all")]
        [TestCase("ALL")]
        public void Parse_IgnoreCase(string value)
        {
            parser.Parse<MemberTypes>(value)
                .Should().BeEquivalentTo(Result.Ok(MemberTypes.All));
        }

        [Test]
        public void Parse_ReturnFailResult_WithIncorrectValue()
        {
            parser.Parse<SpeechPart>("QWE")
                .IsSuccess.Should().BeFalse();
        }
    }
}