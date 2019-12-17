using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    public class SettingsTests
    {
        [TestCase("a", 12, 1)]
        [TestCase("Serif", -1, 1)]
        [TestCase("Serif", 12, 0)]
        public void FontSettingsShould_ThrowException_OnInvalidSetting(string name, float size, float multiplier)
        {
            var fontSettings = new FontSettings(name, FontStyle.Regular, size, multiplier);
            Action action = () => fontSettings.ValidateFontSettings().GetValueOrThrow();
            action.Should().Throw<InvalidOperationException>().WithMessage("No value. Only Error: Invalid font settings");
        }

        [TestCase(0, 100, 50, 50)]
        [TestCase(100, -200, 50, 50)]
        [TestCase(100, 100, -1, 50)]
        [TestCase(100, 100, 50, 0)]
        public void ImageSettingsShould_ThrowException_OnInvalidSetting(int height, int width, float x, float y)
        {
            var imageSettings = new ImageSettings(height, width, x, y);
            Action action = () => imageSettings.ValidateImageSettings().GetValueOrThrow();
            action.Should().Throw<InvalidOperationException>().WithMessage("No value. Only Error: Invalid image settings");
        }

    }
}
