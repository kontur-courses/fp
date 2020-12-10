using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class ImageSizeConverter_Should
    {
        private ImageSizeConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ImageSizeConverter();
        }

        [Test]
        public void ConvertToSize_IsNotSuccess_WhenInvalidNumberParameters()
        {
            var parameters = new[] {1, 1, 5};

            var act = _sut.ConvertToSize(parameters);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToSize_IsNotSuccess_WhenSizeParametersNotPositive()
        {
            var parameters = new[] {0, -1};

            var act = _sut.ConvertToSize(parameters);

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToSize_IsSuccess_WhenSimplePositiveParameters()
        {
            var parameters = new[] {1, 5};

            var act = _sut.ConvertToSize(parameters);

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ConvertToSize_Size_WhenSimplePositiveParameters()
        {
            var parameters = new[] {1, 5};

            var act = _sut.ConvertToSize(parameters).GetValueOrThrow();

            act.Should().Be(new Size(1, 5));
        }
    }
}