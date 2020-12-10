using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.TextProcessing.WordsMeasurer;

namespace TagsCloudTests.UnitTests.TextProcessing_Tests
{
    public class WordMeasurer_Should
    {
        private WordMeasurer _sut;
        private static string _word = "ученик";
        private static Font _font = new Font("arial", 7);

        [SetUp]
        public void SetUp()
        {
            _sut = new WordMeasurer();
        }

        [Test]
        public void GetWordSize_IsNotSuccess_WhenWordIsNull()
        {
            var act = _sut.GetWordSize(null, _font);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWordSize_IsNotSuccess_WhenFontIsNull()
        {
            var act = _sut.GetWordSize(_word, null);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWordSize_IsNotSuccess_WhenWordAndFontAreNull()
        {
            var act = _sut.GetWordSize(null, null);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWordSize_IsSuccess_WhenWordAndFontNotNull()
        {
            var act = _sut.GetWordSize(_word, _font);

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetWordSize_Size_WhenWordAndFontNotNull()
        {
            var sizeF = Graphics.FromHwnd(IntPtr.Zero).MeasureString(_word, _font);
            var expected = new Size((int) Math.Ceiling(sizeF.Width), (int) Math.Ceiling(sizeF.Height));

            var act = _sut.GetWordSize(_word, _font).GetValueOrThrow();

            act.Should().Be(expected);
        }
    }
}