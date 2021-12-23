using FluentAssertions;
using NUnit.Framework;
using TagCloud2.Image;

namespace TagCloud2Tests
{
    public class ImageFormatterHelperTests
    {
        [Test]
        public void GetEncoderInfo_OnJpeg_ShouldReturnJpeg()
        {
            ImageFormatterHelper.GetEncoderInfo("image/jpeg").GetValueOrThrow().CodecName.Should().Be("Built-in JPEG Codec");
        }

        [Test]
        public void GetEncoderInfo_OnPng_ShouldReturnPng()
        {
            ImageFormatterHelper.GetEncoderInfo("image/png").GetValueOrThrow().CodecName.Should().Be("Built-in PNG Codec");
        }

        [Test]
        public void GetEncoderInfo_OnIncorrectCodec_ShouldThrow()
        {
            ImageFormatterHelper.GetEncoderInfo("abobus").Error.Should().NotBeEmpty();
        }
    }
}
