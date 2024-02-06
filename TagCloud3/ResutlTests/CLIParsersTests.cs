using ResultOf;
using System.Drawing;
using TagsCloudContainer.CLI;
using TagsCloudContainer.SettingsClasses;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudContainer.TagCloudBuilder;

namespace ResutlTests
{
    internal class CLIParsersTests
    {
        private CommandLineOptions commandLine;

        [SetUp]
        public void Init()
        {
            commandLine = new();
            commandLine.Filename = Path.GetTempFileName();
            commandLine.Output = Path.GetTempFileName();
            //commandLine.FontFamily = "Arial";
            //commandLine.FontSize = "12";
            //-f sample.txt -o testout.png --exclude excludedWords.txt --font Calibri --colors Gold,Pink,LightGoldenrodYellow --layout normal --fontsize 16 --size 523,532 --background Blue
        }

        [Test]
        public void ParseArgs_ValidFontName_ShouldReturnCorrectFontFamily()
        {
            // Arrange
            commandLine.FontFamily = "Calibri";
            var expectedResult = new FontFamily("Calibri");

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.FontFamily.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ParseArgs_InvalidFontName_ShouldReturnDefaultFontFamily()
        {
            // Arrange
            commandLine.FontFamily = "SomeFont";
            var expectedResult = new FontFamily("Arial");

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.FontFamily.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ParseArgs_ValidFontSize_ShouldReturnCorrectSize()
        {
            // Arrange
            commandLine.FontSize = "19";
            var expectedResult = 19;

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.FontSize.Should().Be(expectedResult);
        }

        [Test]
        public void ParseArgs_InvalidFontSize_ShouldReturnDefaultSize()
        {
            // Arrange
            commandLine.FontSize = "wro1ng";
            var expectedResult = 12;

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.FontSize.Should().Be(expectedResult);
        }

        [Test]
        public void ParseArgs_ValidPointsProvider_ShouldReturnCorrectProvider()
        {
            // Arrange
            commandLine.Layout = "spiral";
            var expected = typeof(SpiralPointsProvider);

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.PointsProvider.GetType().Should().Be(expected);
        }

        [Test]
        public void ParseArgs_InvalidPointsProvider_ShouldReturnDefaultProvider()
        {
            // Arrange
            commandLine.Layout = "badProviderName";
            var expected = typeof(NormalPointsProvider);

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.PointsProvider.GetType().Should().Be(expected);
        }

        [Test]
        public void ParseArgs_ValidSize_ShouldReturnCorrectSize()
        {
            // Arrange
            commandLine.Size = "123,456";
            var expected = new Size(123, 456);

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.Size.Should().Be(expected);
        }

        [Test]
        public void ParseArgs_InvalidSize_ShouldReturnDefaultSize()
        {
            // Arrange
            commandLine.Size = "bad size";
            var expected = new Size(800, 600);

            // Act
            var actualResult = CommandLineOptions.ParseArgs(commandLine);

            // Assert
            actualResult.DrawingSettings.Size.Should().Be(expected);
        }
    }
}
