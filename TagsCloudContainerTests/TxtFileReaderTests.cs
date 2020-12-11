using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;

namespace TagsCloudContainerTests
{
    internal class TxtFileReaderTests
    { 
        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");

        [Test]
        public void TxtReader_ReadLinesWithoutError_IfFileExists()
        {
            var result = new TxtFileReader(txtFilePath).ReadLines();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo("Это", "Txt", "Файл");
        }

        [Test]
        public void TxtReader_ShouldReturnResultWithError_IfFileDoesNotExist()
        {
            new TxtFileReader("notExistedPath.txt")
                .ReadLines()
                .Error
                .Should()
                .BeEquivalentTo("Input file is not found");
        }
    }
}