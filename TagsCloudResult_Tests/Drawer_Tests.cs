using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class DrawerTests
    {
        private static AppSettings settings = AppSettingsForTests.Settings;

        private static IEnumerable<(Rectangle, LayoutWord)> words = new[]
        {
            (new Rectangle(20, 20, 10, 10),
                new LayoutWord("First", new SolidBrush(Color.Red), new Font("Arial", 7), new Size(4, 6))
            )
        };

        [Test]
        public void ShouldReturnBitmap_IfGetCorrectInput()
        {
            var actual = CloudDrawer.Draw(words, settings);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnError_IfGetUndefinedColor()
        {
            var badSetting = new AppSettings(
                new ImageSetting(240, 240, "ads", "png", "test"),
                default(WordSetting),
                default(AlgorithmsSettings), "");
            var actual = CloudDrawer.Draw(words, badSetting);

            actual.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ShouldReturnError_IfGetBadSize()
        {
            var badSetting =  new AppSettings(
                new ImageSetting(-240, 240, "Black", "png", "test"),
                default(WordSetting),
                default(AlgorithmsSettings), "");
            var actual = CloudDrawer.Draw(words, badSetting);

            actual.IsSuccess.Should().BeFalse();
        }
    }
}