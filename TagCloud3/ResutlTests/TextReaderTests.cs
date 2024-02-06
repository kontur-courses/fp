using FluentAssertions;
using ResultOf;
using TagsCloudContainer.TextTools;

namespace ResutlTests
{
    public class TextReaderTests
    {
        [Test]
        public void ReadText_ShouldReturnCorrectResult_WhenFileExists()
        {
            // Arrange
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, "Test text");
            var textFileReader = new TextFileReader();

            // Act
            var result = textFileReader.ReadText(filePath);

            // Assert
            result.Should().BeOfType<Result<string>>();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().Be("Test text");
        }

        [Test]
        public void ReadText_ShouldReturnFailureResult_WhenFileDoesNotExist()
        {
            // Arrange
            var filePath = "non_existent_file.txt";
            var textFileReader = new TextFileReader();

            // Act
            var result = textFileReader.ReadText(filePath);

            // Assert
            result.Should().BeOfType<Result<string>>();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
        }
    }
}