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

        [Test]
        public void WordReader_ShouldReadLines()
        {
            new WordFileReader(docxFilePath)
                .ReadLines()
                .GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo("Это", "Docx", "Файл", "");
        }
    }
}
