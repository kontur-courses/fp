using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using FontConverter = TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont.FontConverter;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class FontConverter_Should
    {
        private FontConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FontConverter();
        }

        [Test]
        public void ConvertToFont_IsNotSuccess_WhenInvalidNumberParameters()
        {
            var parameters = new[] {"arial", "1", "1"};

            var act = _sut.ConvertToFont(parameters);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToFont_IsNotSuccess_WhenSizeNotPositive()
        {
            var parameters = new[] {"arial", "0"};

            var act = _sut.ConvertToFont(parameters);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToFont_IsNotSuccess_WhenUnknownFontName()
        {
            var parameters = new[] {"arialsss", "10"};

            var act = _sut.ConvertToFont(parameters);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToFont_IsSuccess_WhenValidParameters()
        {
            var parameters = new[] {"arial", "10"};

            var act = _sut.ConvertToFont(parameters);

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ConvertToFont_Font_WhenValidParameters()
        {
            var parameters = new[] {"arial", "10"};

            var act = _sut.ConvertToFont(parameters).GetValueOrThrow();

            act.Should().Be(new Font("arial", 10));
        }
    }
}