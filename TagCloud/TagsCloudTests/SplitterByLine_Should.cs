using System;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using TagsCloud.Splitters;
using TagsCloud;

namespace TagsCloudTests
{
    class SplitterByLine_Should
    {
        private SplitterByLine splitterByLine;

        [SetUp]
        public void SetUp()
        {
            splitterByLine = new SplitterByLine();
        }

        [Test]
        public void SplitText_Should_SplitStringByEnviromentNewLine()
        {
            var words = new List<string>() { "съешь", "ещё", "этих", "мягких", "французских", "булок", "да", "выпей", "чаю"};
            var inputString = string.Join(Environment.NewLine, words);
            var result = splitterByLine.SplitText(inputString);
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(words);
        }

        [Test]
        public void SplitText_Should_RemoveEmptyLines()
        {
            var words = new List<string>() { "съешь", "", "ещё", " ", "этих", "  ", "мягких", "\n", "французских", "булок", "да", "выпей", "чаю" };
            var wordsWithoutEmptyLines = new List<string>() { "съешь", "ещё", "этих", "мягких", "французских", "булок", "да", "выпей", "чаю" };
            var inputString = string.Join(Environment.NewLine, words);
            var result = splitterByLine.SplitText(inputString);
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(wordsWithoutEmptyLines);
        }

        [Test]
        public void SplitText_Should_ReturnResultFail_When_InputTextEqualNull()
        {
            var result = splitterByLine.SplitText(null);
            result.IsSuccess.Should().BeFalse();
        }
    }
}
