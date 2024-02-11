using FluentAssertions;
using System.Text;
using TagsCloudContainer.TextTools;

namespace TagsCloudTests
{
    [TestFixture]
    public class FileReadingTests
    {
        private string tempFilePath;
        private const string testText = "Expected text from the file";

        [SetUp]
        public void Setup()
        {
            tempFilePath = CreateTempFile();
        }

        [Test]
        public void ReadTextFromFile_ShouldReturnCorrectText()
        {
            var filePath = tempFilePath;
            var expectedText = testText;

            var actualResult = TextFileReader.ReadText(filePath);

            actualResult.IsSuccess.Should().BeTrue();
            actualResult.GetValueOrDefault().Should().Be(expectedText);
        }

        [Test]
        public void ReadTextFromFile_ShouldReturnError_WhenFileDoesNotExist()
        {
            var filePath = "nonexistent_file.txt";

            var actualResult = TextFileReader.ReadText(filePath);

            actualResult.IsSuccess.Should().BeFalse();
        }

        private string CreateTempFile()
        {
            var tempFile = Path.GetTempFileName();
            using (var streamWriter = new StreamWriter(tempFile, false, Encoding.UTF8))
            {
                streamWriter.Write(testText);
            }
            return tempFile;
        }
    }
}