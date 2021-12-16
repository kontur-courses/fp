using System.Collections.Generic;
using System.Drawing.Imaging;
using NUnit.Framework;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Tests.FluentAssertionsExtensions;

namespace TagsCloud.Tests
{
    public class ImageFormatParserTest
    {
        private ImageFormatParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ImageFormatParser();
        }

        [Test]
        public void Parse_ReturnFailResult_WithIncorrectValue()
        {
            parser.Parse("esf")
                .Should().BeFailed();
        }

        [TestCaseSource(nameof(ParseCases))]
        public void Parse_ReturnFormat_WithCorrectValue(string value, ImageFormat expected)
        {
            parser.Parse(value)
                .Should().BeOk(expected);
        }

        public static IEnumerable<TestCaseData> ParseCases()
        {
            yield return new TestCaseData("bmp", ImageFormat.Bmp) {TestName = "Lowercase name"};
            yield return new TestCaseData("PNG", ImageFormat.Png) {TestName = "Uppercase name"};
        }
    }
}