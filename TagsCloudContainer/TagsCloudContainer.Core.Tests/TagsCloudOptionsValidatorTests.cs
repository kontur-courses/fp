using DocumentFormat.OpenXml.Wordprocessing;
using FakeItEasy;
using FluentAssertions;
using TagsCloudContainer.Core.CLI;
using TagsCloudContainer.Core.Options;

namespace TagsCloudContainer.Core.Tests
{
    [TestFixture]
    public class TagsCloudValidatorTests
    {
        private CommandLineOptions opt;
        [SetUp]
        public void SetUp()
        {
            opt = A.Fake<CommandLineOptions>();
        }
        [TestCase("-1", "100")]
        [TestCase("100", "-1")]
        [TestCase("0", "100")]
        [TestCase("100", "0")]
        public void ValidateImageOptions_WhenIncorrectSize_ShouldBeErrorMessage(string width, string height)
        {
            opt.Width = width;
            opt.Height = height;
            var result = TagsCloudOptionsValidator.ValidateImageOptions(opt);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect image size.");
        }

        [Test]
        public void ValidateImageOptions_WhenCorrectSize_ShouldBeTrue()
        {
            opt.Width = "500";
            opt.Height = "500";
            var result = TagsCloudOptionsValidator.ValidateImageOptions(opt);

            result.IsSuccess.Should().BeTrue();
        }

        [TestCase("integer", "0")]
        [TestCase("0", "integer")]
        [TestCase("100.2", "110")]
        [TestCase("100", "110.3")]
        public void ValidateImageOptions_WhenSizeIsNotInt_ShouldBeFalse(string width, string height)
        {
            opt.Width = width;
            opt.Height = height;
            var result = TagsCloudOptionsValidator.ValidateImageOptions(opt);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect image size.");
        }

        [TestCase("notacolor")]
        [TestCase("rgba(a,0,0,0)")]
        [TestCase("rgba(1, 0, 0, 0)")]
        [TestCase("rgba(0,a,0,0)")]
        [TestCase("rgba(0,0,a,0)")]
        [TestCase("rgba(0,0,0,a)")]
        [TestCase("rgba(0,-10,0,0)")]
        [TestCase("rgba(0,0)")]
        public void ValidateFontOptions_WhenIncorrectFont_ShouldBeFalse(string color)
        {
            opt.FontColor = color;
            opt.FontFamily = "Arial";
            var result = TagsCloudOptionsValidator.ValidateFontOptions(opt);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect font color.");
        }

        [Test]
        public void ValidateFontOptions_WhenIncorrectFamily_ShouldBeFalse()
        {
            opt.FontColor = "argb(1,0,0,0)";
            opt.FontFamily = "NotAFamily";
            var result = TagsCloudOptionsValidator.ValidateFontOptions(opt);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($"Font family {opt.FontFamily} doesn't exist.");
        }

        [Test]
        public void voidValidateFontOptions_WhenCorrectFontShouldBeTrue()
        {
            opt.FontColor = "argb(1,0,0,0)";
            opt.FontFamily = "Arial";
            var result = TagsCloudOptionsValidator.ValidateFontOptions(opt);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
