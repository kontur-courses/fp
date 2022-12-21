using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Validators;

namespace TagsCloudTests.ValidatorsShould
{
    [TestFixture]
    public class SettingsValidatorShould
    {
        private SettingsValidator validator = new SettingsValidator();

        [Test]
        public void VerifyFont_NotInstalledFont()
        {
            var fontName = "Not installed font";

            var res = validator.VerifyFont(fontName);

            ClientValidatorShould.ErrorValidate(res, "Font was not found");
        }

        [Test]
        public void VerifyFont_InstalledFont()
        {
            var fontName = "Arial";

            var res = validator.VerifyFont(fontName);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("Arial");
        }

        [TestCase(0, 100)]
        [TestCase(0, 0)]
        [TestCase(-10, 100)]
        [TestCase(-720, -1080)]
        public void VerifyPictureSize_SizeWithNegativeOrZeroEdge(int width, int height)
        {
            var res = validator.VerifyPictureSize(new Size(width, height));

            ClientValidatorShould.ErrorValidate(res, "The size of the picture is wrong");
        }

        [Test]
        public void VerifyPictureSize_NormalSize()
        {
            var size = new Size(720, 1024);

            var res = validator.VerifyPictureSize(size);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(size);
        }
    }
}