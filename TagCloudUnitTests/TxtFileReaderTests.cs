using FluentAssertions;
using NUnit.Framework;
using TagCloud.FileReader;

namespace TagCloudUnitTests
{
    public class TxtFileReaderTests
    {
        private TxtFileReader fileReader;

        private readonly string solutionDirectory = DirectoryHandler.GetSolutionDirectory().FullName;

        [SetUp]
        public void Setup()
        {
            fileReader = new TxtFileReader();
        }

        [Test]
        public void ReadAllText_ReturnsAllFileText_WhenFileExists()
        {
            var expectedText = "This is txt file.";

            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\TestText.txt");

            actualText.IsSuccess.Should().BeTrue();

            actualText.GetValueOrThrow().Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileDoesNotExist()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\blablabla.txt");

            actualText.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadAllText_IsNotSuccess_WhenFileHasInvalidFormat()
        {
            var actualText = fileReader.ReadAllText(solutionDirectory + @"\TestTextFiles\TestText.docx");

            actualText.IsSuccess.Should().BeFalse();
        }
    }
}
