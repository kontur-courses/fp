using FluentAssertions;
using System.Drawing;
using TagsCloudContainer.CLI;
using TagsCloudContainer.TagCloudBuilder;
using TagsCloudVisualization;

namespace ResultTests
{
    public class CLIParsersTests
    {
        private CommandLineOptions commandLine;

        [SetUp]
        public void Init()
        {
            commandLine = new();
            commandLine.Filename = Path.GetTempFileName();
            commandLine.Output = Path.GetTempFileName();
        }

        [Test]
        public void ParseArgs_ValidFontName_ShouldReturnCorrectFontFamily()
        {
            commandLine.FontFamily = "Calibri";
            var expectedResult = new FontFamily("Calibri");

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.FontFamily.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ParseArgs_InvalidFontName_ShouldReturnDefaultFontFamily()
        {
            commandLine.FontFamily = "SomeFont";
            var expectedResult = new FontFamily("Arial");

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.FontFamily.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ParseArgs_ValidFontSize_ShouldReturnCorrectSize()
        {
            commandLine.FontSize = "19";
            var expectedResult = 19;

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.FontSize.Should().Be(expectedResult);
        }

        [Test]
        public void ParseArgs_InvalidFontSize_ShouldReturnDefaultSize()
        {
            commandLine.FontSize = "wro1ng";
            var expectedResult = 12;

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.FontSize.Should().Be(expectedResult);
        }

        [Test]
        public void ParseArgs_ValidPointsProvider_ShouldReturnCorrectProvider()
        {
            commandLine.Layout = "spiral";
            var expected = typeof(SpiralPointsProvider);

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.PointsProvider.GetType().Should().Be(expected);
        }

        [Test]
        public void ParseArgs_InvalidPointsProvider_ShouldReturnDefaultProvider()
        {
            commandLine.Layout = "badProviderName";
            var expected = typeof(NormalPointsProvider);

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.PointsProvider.GetType().Should().Be(expected);
        }

        [Test]
        public void ParseArgs_ValidSize_ShouldReturnCorrectSize()
        {
            commandLine.Size = "123,456";
            var expected = new Size(123, 456);

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.Size.Should().Be(expected);
        }

        [Test]
        public void ParseArgs_InvalidSize_ShouldReturnDefaultSize()
        {
            commandLine.Size = "bad size";
            var expected = new Size(800, 600);

            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            actualResult.DrawingSettings.Size.Should().Be(expected);
        }
    }
}