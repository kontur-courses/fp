using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization;

namespace TagsCloud.Tests
{
    internal class FontSettings_Tests
    {
        private FontSettings settings;
        private int minFontSize = 8;
        private int maxFontSize = 50;

        [SetUp]
        public void SetUp()
        {
            settings = new FontSettings();

            settings.FontFamilyName = "Arial";
            settings.FontSize = 10;
        }

        [Test]
        public void MainFont_ReturnArialFont_WhenSetArialFontFamily()
        {
            settings.Font.GetValueOrThrow()
                .Should().Be(new Font("Arial", 10));
        }

        [Test]
        public void MainFont_ReturnFontWithMinFontSize_WhenSetFontSizeLessThenMin()
        {
            settings.FontSize = -10;

            settings.Font.GetValueOrThrow()
                .Should().Be(new Font("Arial", minFontSize));
        }

        [Test]
        public void MainFont_ReturnFontWithMaxFontSize_WhenSetFontSizeMoreThanMax()
        {
            settings.FontSize = 100;

            settings.Font.GetValueOrThrow()
                .Should().Be(new Font("Arial", maxFontSize));
        }

        [Test]
        public void MainFont_ThrowException_WhenSetInvalidFontFamilyName()
        {
            settings.FontFamilyName = "wegweg";

            Assert.Throws<InvalidOperationException>(() => settings.Font.GetValueOrThrow());
        }
    }
}
