using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;

namespace TagsCloudContainerTests
{
    internal class WordFileReaderTests
    {
        private readonly string docxFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.docx");

        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");

        [Test]
        public void WordReader_ShouldReadLines_IfFileExists()
        {
            var result = new WordFileReader(docxFilePath).ReadLines();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo("Это", "Docx", "Файл", "");
        }

        [Test]
        public void DocxReader_ShouldReturnResultWithError_IfFileDoesNotExist()
        {
            new WordFileReader("notExistedPath.docx")
                .ReadLines()
                .Error
                .Should()
                .BeEquivalentTo("Input file is not found");
        }

        [Test]
        public void DocxReader_ShouldReturnResultWithError_IfCannotReadFile()
        {
            new WordFileReader(txtFilePath)
                .ReadLines()
                .Error
                .Should()
                .Contain("Can't read input file");
        }
    }
}