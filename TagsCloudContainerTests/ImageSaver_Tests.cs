using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.ImageSavers;

namespace TagsCloudContainerTests
{
    public class ImageSaverTests
    {
        private readonly IImageSaver imageSaver = new ImageSaver();

        [Test]
        public void Save_ReturnsFailResult_WhenFormatIsNotSupported()
        {
            var result = imageSaver.Save(new Bitmap(5, 5), "image.asd");

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(".asd format is not supported");
        }
        
        [Test]
        public void Save_ReturnsFailResult_WhenPathContainsInvalidChars()
        {
            var result = imageSaver.Save(new Bitmap(5, 5), "ima|ge.png");

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Image path contains characters that are not allowed in path names");
        }

        [TestCase("image.png", TestName = "WhenPngImage")]
        [TestCase("image.bmp", TestName = "WhenBmpImage")]
        [TestCase("image.jpg", TestName = "WhenJpgImage")]
        [TestCase("image.gif", TestName = "WhenGifImage")]
        public void Save_WorksCorrectlyWithSupportedFormats(string path)
        {
            var saver = new ImageSaver();
            var image = new Bitmap(5, 5);

            saver.Save(image, path);

            File.Exists(path).Should().BeTrue();

            File.Delete(path);
        }
    }
}