using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudApp.Parsers;

namespace TagsCloud.Tests
{
    public class ArgbColorParserTests
    {
        private ArgbColorParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ArgbColorParser();
        }

        [TestCase("#sef")]
        [TestCase("1 1 1 1")]
        [TestCase("1 1 1")]
        public void Parse_WithIncorrectValue_ReturnFailResult(string value)
        {
            parser.Parse(value)
                .IsSuccess.Should().BeFalse();
        }

        [TestCaseSource(nameof(ParseReturnColorFromCases))]
        public void Parse_ReturnColor_From(string value, Color expected)
        {
            var actual = parser.Parse(value).GetValueOrThrow();
            actual
                .ToArgb()
                .Should().Be(expected.ToArgb(), $"actual: {actual}, expected: {expected}");
        }


        private static IEnumerable<TestCaseData> ParseReturnColorFromCases()
        {
            yield return new TestCaseData("#FFFF0000", Color.Red) {TestName = "ARGB Hex"};
            yield return new TestCaseData("#FF0000", Color.Red) {TestName = "RGB Hex"};
            yield return new TestCaseData("#00F", Color.Blue) {TestName = "Short RGB Hex"};
            yield return new TestCaseData("AliceBlue", Color.AliceBlue) {TestName = "Html name"};
        }
    }
}