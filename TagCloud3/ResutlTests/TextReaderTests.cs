using FluentAssertions;
using ResultOf;
using TagsCloudContainer.TextTools;

namespace ResultTests
{
    public class TextReaderTests
    {
        [Test]
        public void ReadText_ShouldReturnCorrectResult_WhenFileExists()
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, "Test text");

            var result = TextFileReader.ReadText(filePath);

            result.Should().BeOfType<Result<string>>();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrDefault().Should().Be("Test text");
        }

        [Test]
        public void ReadText_ShouldReturnFailureResult_WhenFileDoesNotExist()
        {
            var filePath = "non_existent_file.txt";

            var result = TextFileReader.ReadText(filePath);

            result.Should().BeOfType<Result<string>>();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
        }
    }
}