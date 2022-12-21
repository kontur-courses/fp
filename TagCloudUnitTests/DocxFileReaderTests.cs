using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using TagCloud.FileReader;

namespace TagCloudUnitTests
{
    [TestFixture]
    public class DocxFileReaderTests
    {
        private DocxFileReader fileReader;

        [SetUp]
        public void Setup()
        {
            fileReader = new DocxFileReader();
        }

        [Test]
        public void ReadAllText_ReturnsAllFileText_WhenFileExists()
        {
            var expectedText = "This is docx file."; 

            var actualText = fileReader.ReadAllText(@"TestTextFiles\TestText.docx");

            actualText.IsSuccess.Should().BeTrue();

            actualText.GetValueOrThrow().Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileDoesNotExist()
        {
            var actualText = fileReader.ReadAllText("blablabla");

            actualText.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileHasInvalidFormat()
        {
            var actualText = fileReader.ReadAllText(@"TestTextFiles\TestText.doocc");

            actualText.IsSuccess.Should().BeFalse();
        }
    }
}
