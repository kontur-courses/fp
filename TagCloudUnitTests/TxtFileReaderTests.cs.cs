using FluentAssertions;
using NUnit.Framework;
using TagCloud.FileReader;

namespace TagCloudUnitTests
{

    public class TxtFileReaderTests
    {
        private TxtFileReader fileReader;

        [SetUp]
        public void Setup()
        {
            fileReader = new TxtFileReader();
        }

        [Test]
        public void ReadAllText_ReturnsAllFileText_WhenFileExists()
        {
            var expectedText = "This is txt file.";

            var actualText = fileReader.ReadAllText(@"TestTextFiles\TestText.txt");

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
            var actualText = fileReader.ReadAllText(@"TestTextFiles\TestText.ttxtt");

            actualText.IsSuccess.Should().BeFalse();
        }
    }
}
