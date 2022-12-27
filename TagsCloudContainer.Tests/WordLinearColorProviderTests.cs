using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.WordColorProviders;
using TagsCloudContainer.Infrastructure.WordColorProviders.Factories;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class WordLinearColorProviderTests
    {
        private static IWordColorProviderFactory colorProviderFactory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            colorProviderFactory = new WordLinearColorProviderFactory();
        }

        [Test]
        public void GetColor_Should_ReturnFailedResult_WhenWordDoesntExist()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var settings = new WordColorSettings(){ MinFrequencyColor = Color.Black, MaxFrequencyColor = Color.Black, WordFrequencies = wordFrequencies };
            var colorProvider = colorProviderFactory.CreateDefault(settings);

            var result = colorProvider.GetColor("awesome");

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void GetColor_Should_ReturnMinFrequencyColor_WhenWordHasMinFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var expectedColor = Color.Black;
            var settings = new WordColorSettings() { MinFrequencyColor = expectedColor, MaxFrequencyColor = Color.White, WordFrequencies = wordFrequencies };
            var colorProvider = colorProviderFactory.CreateDefault(settings);

            var actual = colorProvider.GetColor("some");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.ToArgb().Should().Be(expectedColor.ToArgb());
        }

        [Test]
        public void GetColor_Should_ReturnMaxFrequencyColor_WhenWordHasMaxFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var expectedColor = Color.White;
            var settings = new WordColorSettings() { MinFrequencyColor = Color.Black, MaxFrequencyColor = expectedColor, WordFrequencies = wordFrequencies };
            var colorProvider = colorProviderFactory.CreateDefault(settings);

            var actual = colorProvider.GetColor("text");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.ToArgb().Should().Be(expectedColor.ToArgb());
        }
          
        [Test]
        public void GetColor_Should_ReturnAverageFrequencyColor_WhenWordHasAverageFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "awesome", 2 }, { "text", 3 } };
            var expectedColor = Color.FromArgb(128, 128, 128);
            var settings = new WordColorSettings() { MinFrequencyColor = Color.Black, MaxFrequencyColor = Color.White, WordFrequencies = wordFrequencies };
            var colorProvider = colorProviderFactory.CreateDefault(settings);

            var actual = colorProvider.GetColor("awesome");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.ToArgb().Should().Be(expectedColor.ToArgb());
        }

        [Test]
        public void GetColor_Should_ReturnFrequencyColor_WhenMinFrequencyColorEqualsToMax()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "awesome", 2 }, { "text", 3 } };
            var expectedColor = Color.Blue;
            var settings = new WordColorSettings() { MinFrequencyColor = expectedColor, MaxFrequencyColor = expectedColor, WordFrequencies = wordFrequencies };
            var colorProvider = colorProviderFactory.CreateDefault(settings);

            var actual = colorProvider.GetColor("awesome");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.ToArgb().Should().Be(expectedColor.ToArgb());
        }
    }
}