using FluentAssertions;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Readers;

namespace TagsCloudContainerTests
{

    [TestFixture]
    public class FileReaderTests
    {
        private IFileReader fileReader;

        private Dictionary<string, Func<IFileReader>> readerCreators;

        public FileReaderTests()
        {
            readerCreators = new Dictionary<string, Func<IFileReader>>(StringComparer.OrdinalIgnoreCase)
        {
            { ".docx", () => new DocxReader() },
            { ".doc", () => new DocReader() },
            { ".txt", () => new TxtReader() }
        };
        }

        [SetUp]
        public void SetUp()
        {
            fileReader = new TxtReader();
        }

        [Test]
        [TestCase("text.txt")]
        [TestCase("text.docx")]
        [TestCase("text.doc")]
        public void ValidFilePath_ReturnsNonEmptyList(string fileName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var relativeFilePath = Path.Combine("src", fileName);
            var filePath = Path.Combine(currentDirectory, relativeFilePath);

            fileReader = GetFileReader(filePath);

            var result = fileReader.ReadWords(filePath);

            result.OnSuccess(words =>
            {
                words.Should().NotBeNull().And.NotBeEmpty();
            });

            result.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        private IFileReader GetFileReader(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            if (readerCreators.TryGetValue(extension, out var creator))
                return creator();
            else
                throw new NotSupportedException($"File extension '{extension}' is not supported.");
        }

        [Test]
        public void InvalidFilePath_ReturnsEmptyList()
        {
            var filePath = "path/to/invalid/file.txt";

            var result = fileReader.ReadWords(filePath);

            result.OnSuccess(words =>
            {
                Assert.Fail("Test failed. Expected an error.");
            });

            result.OnFail(error =>
            {
                error.Should().NotBeNullOrEmpty();
            });
        }

        [Test]
        public void WithSpecificContent_ReturnsExpectedWords()
        {
            var filePath = "src/boring_words.txt";
            var content = "a an the";
            File.WriteAllText(filePath, content);

            var result = fileReader.ReadWords(filePath);

            result.OnSuccess(words =>
            {
                words.Should().Contain("a", "an", "the");
            });

            result.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        [Test]
        public void EmptyFile_ReturnsEmptyList()
        {
            var filePath = @"..\\..\\..\\src\\empty.txt";
            File.WriteAllText(filePath, string.Empty);

            var result = fileReader.ReadWords(filePath);

            result.OnSuccess(words =>
            {
                words.Should().BeEmpty();
            });

            result.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }
    }
}
