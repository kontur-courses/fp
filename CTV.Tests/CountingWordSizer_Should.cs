using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CTV.Common;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace CTV.Tests
{
    //#FIXME
    [TestFixture]
    public class CountingWordSizer_Should
    {
        private readonly FrequencyBasedWordSizer sizer = new();
        private Graphics g;

        [SetUp]
        public void OnSetup()
        {
            g = A.Fake<Graphics>();
        }

        [TestCase(0, new[] {"abc"}, TestName = "When font size is zero")]
        [TestCase(-1, new[] {"abc"}, TestName = "When font size is negative")]
        [TestCase(1, null, TestName = "When input words array is null")]
        [TestCase(1, new[] {(string) null}, TestName = "When words contains null")]
        public void Throw_When(float fontSize, string[] words)
        {
            var font = CreateDefaultFont(fontSize);
            Action action = () => sizer.Convert(words, font, g);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ReturnEmpty_WhenNoWordsGiven()
        {
            var words = Array.Empty<string>();
            var font = CreateDefaultFont(20);

            var result = sizer.Convert(words, font, g);

            result.Should().BeEmpty();
        }


        [Test]
        public void ReturnSizeFontSizeDueToFrequency()
        {
            var words = new[] {"a", "a", "a", "b", "b", "c"};
            var fontSize = 20;
            var font = CreateDefaultFont(fontSize);
            var returnedSize = new Size(10, 20);
            A.CallTo(() => g.MeasureString(null, null))
                .WithAnyArguments()
                .Returns(returnedSize);

            var expected = new List<SizedWord>
            {
                new("a", CreateDefaultFont(3f / 6f * fontSize), returnedSize),
                new("b", CreateDefaultFont(2f / 6f * fontSize), returnedSize),
                new("c", CreateDefaultFont(1f / 6f * fontSize), returnedSize)
            };

            var result = sizer.Convert(words, font, g);

            result.Should().BeEquivalentTo(
                expected,
                config => config.WithoutStrictOrdering());
        }

        //#FIXME Добавить тесты на WordSize


        private Font CreateDefaultFont(float size)
        {
            return new Font(FontFamily.GenericSerif, size, FontStyle.Bold);
        }
    }
}