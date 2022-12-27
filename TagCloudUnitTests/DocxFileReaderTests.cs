using FluentAssertions;
using NUnit.Framework;
using TagCloud.FileReader;

namespace TagCloudUnitTests
{
    [TestFixture]
    public class DocxFileReaderTests
    {
        private DocxFileReader fileReader;

        private readonly string solutionDirectory = DirectoryHandler.GetSolutionDirectory().FullName;

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
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\blablabla.docx");

            actualText.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileHasInvalidFormat()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\TestText.txt");

            actualText.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileHasInvalidContent()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\NotDocxTestText.docx");

            actualText.IsSuccess.Should().BeFalse();
        }
    }
}