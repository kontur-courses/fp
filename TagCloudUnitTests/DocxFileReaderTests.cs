using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System;
using TagCloud.FileReader;

namespace TagCloudUnitTests
{
    [TestFixture]
    public class DocxFileReaderTests
    {
        private DocxFileReader fileReader;

        private readonly string solutionDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

        [SetUp]
        public void Setup()
        {
            fileReader = new DocxFileReader();
        }

        [Test]
        public void ReadAllText_ReturnsAllFileText_WhenFileExists()
        {
            var expectedText = "This is docx file.";

            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\TestText.docx");

            actualText.IsSuccess.Should().BeTrue();

            actualText.GetValueOrThrow().Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileDoesNotExist()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\blablabla");

            actualText.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileHasInvalidFormat()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\TestText.txt");

            actualText.IsSuccess.Should().BeFalse();
        }
    }
}
