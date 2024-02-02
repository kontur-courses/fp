using FluentAssertions;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Readers;

namespace TagsCloudContainerTests
{

    [TestFixture]
    public class FileReaderTests
    {
        private IFileReader fileReader;

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

            IFileReader fileReader = GetFileReader(filePath);

            var result = fileReader.ReadWords(filePath);

            result.OnSuccess(words =>
            {
                foreach (var word in words)
                {
                    Console.WriteLine(word);
                }

                words.Should().NotBeNull().And.NotBeEmpty();
            });

            result.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        private IFileReader GetFileReader(string filePath)
        {
            if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
            {
                return new DocxReader();
            }
            else if (Path.GetExtension(filePath).Equals(".doc", StringComparison.OrdinalIgnoreCase))
            {
                return new DocReader();
            }
            else
            {
                return new TxtReader();
            }
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
