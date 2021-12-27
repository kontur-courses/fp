using System;
using System.Collections.Generic;
using System.Drawing;
using CTV.Common;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace CTV.Tests
{
    [TestFixture]
    public class CountingWordSizer_Should
    {
        private readonly FrequencyBasedWordSizer sizer = new();
        private Func<string, Font, Size> getWordSize;

        [SetUp]
        public void OnSetup()
        {
            getWordSize = A.Fake<Func<string, Font, Size>>();
        }

        [TestCase(1, null, TestName = "When input words array is null")]
        [TestCase(1, new[] {(string) null}, TestName = "When words contains null")]
        public void Throw_When(float fontSize, string[] words)
        {
            var font = CreateDefaultFont(fontSize);
            Action action = () => sizer.Convert(words, font, getWordSize);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ThrowArgumentNullException_WhenFontIsNull()
        {
            var words = new[] {"hello", "world"};
            Action action = () => sizer.Convert(words, null, getWordSize);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ReturnEmpty_WhenNoWordsGiven()
        {
            var words = Array.Empty<string>();
            var font = CreateDefaultFont(20);

            var result = sizer.Convert(words, font, getWordSize);

            result.Should().BeEmpty();
        }


        [Test]
        public void ReturnFontSizeDueToFrequency()
        {
            var words = new[] {"a", "a", "a", "b", "b", "c"};
            var fontSize = 20f;
            var font = CreateDefaultFont(fontSize);
            var returnedSize = new Size(10, 20);
            A.CallTo(() => getWordSize(null, null))
                .WithAnyArguments()
                .Returns(returnedSize);

            var expected = new List<SizedWord>
            {
                new("a", CreateDefaultFont(3f / 6f * fontSize), returnedSize),
                new("b", CreateDefaultFont(2f / 6f * fontSize), returnedSize),
                new("c", CreateDefaultFont(1f / 6f * fontSize), returnedSize)
            };

            var result = sizer.Convert(words, font, getWordSize);

            result.Should().BeEquivalentTo(
                expected,
                config => config.WithoutStrictOrdering());
        }

        [Test]
        public void CallGetWordSizeFunctionWithChangedSize()
        {
            var words = new[] {"a", "a", "a", "b", "b", "c"};
            var fontSize = 20f;
            var font = CreateDefaultFont(fontSize);
            var returnedSize = new Size(10, 20);
            A.CallTo(() => getWordSize("a", CreateDefaultFont(3f / 6f * fontSize)))
                .Returns(returnedSize);
            A.CallTo(() => getWordSize("b", CreateDefaultFont(2f / 6f * fontSize)))
                .Returns(returnedSize);
            A.CallTo(() => getWordSize("c", CreateDefaultFont(1f / 6f * fontSize)))
                .Returns(returnedSize);

            var expected = new List<SizedWord>
            {
                new("a", CreateDefaultFont(3f / 6f * fontSize), returnedSize),
                new("b", CreateDefaultFont(2f / 6f * fontSize), returnedSize),
                new("c", CreateDefaultFont(1f / 6f * fontSize), returnedSize)
            };

            var result = sizer.Convert(words, font, getWordSize);

            result.Should().BeEquivalentTo(
                expected,
                config => config.WithoutStrictOrdering());
        }

        private Font CreateDefaultFont(float size)
        {
            return new Font(FontFamily.GenericSerif, size, FontStyle.Bold);
        }
    }
}