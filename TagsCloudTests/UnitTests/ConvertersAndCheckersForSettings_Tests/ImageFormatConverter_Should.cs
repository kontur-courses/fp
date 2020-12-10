using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class ImageFormatConverter_Should
    {
        private ImageFormatConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ImageFormatConverter();
        }

        [Test]
        public void ConvertToImageFormat_IsNotSuccess_WhenUnknownFormatFromString()
        {
            var act = _sut.ConvertToImageFormat("fff");

            act.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ConvertToImageFormat_IsSuccess_WhenKnownFormatFromString()
        {
            var act = _sut.ConvertToImageFormat("bmp");

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ConvertToImageFormat_ImageFormat_WhenKnownFormatFromString()
        {
            var act = _sut.ConvertToImageFormat("bmp").GetValueOrThrow();

            act.Should().Be(ImageFormat.Bmp);
        }
    }
}