using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class ImageCreatorTests
    {
        private AppSettings settings = AppSettingsForTests.Settings;

        [Test]
        public void SaveImage_IfGoodFormat()
        {
            var bitmap = new Bitmap(24, 25);
            var actual = ImageCreator.Save(bitmap, settings);

            actual.IsSuccess.Should().BeTrue();
            System.IO.File.Exists($"{settings.ImageSetting.Name}.{settings.ImageSetting.Format}");
            System.IO.File.Delete($"{settings.ImageSetting.Name}.{settings.ImageSetting.Format}");
        }
    }
}