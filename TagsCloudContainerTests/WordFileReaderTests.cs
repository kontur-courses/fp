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
        public void TxtReader_ShouldReadLines()
        {
            new WordFileReader(docxFilePath)
                .ReadLines()
                .ToArray()
                .Should()
                .BeEquivalentTo("Это", "Docx", "Файл", "");
        }
    }
}
