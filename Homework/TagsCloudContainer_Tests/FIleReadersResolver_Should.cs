using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.FileReader;

namespace TagsCloudContainer_Tests
{
    [TestFixture]
    public class FIleReadersResolver_Should
    {
        private readonly IFileReader reader = A.Fake<IFileReader>();
        private FileReadersResolver sut;


        [TestCase(".txt", ".txt", TestName = "when extension is supported", ExpectedResult = true)]
        [TestCase(".png", ".txt", TestName = "when unsupported extension", ExpectedResult = false)]
        public bool ReturnsCorrectResult(string fileReaderExtension, string fileExtension)
        {
            A.CallTo(() => reader.Extension).Returns(fileReaderExtension);
            sut = new FileReadersResolver(reader);
            return sut.Get($"hello{fileExtension}").IsSuccess;
        }

        [TestCase("hello.png", @"Format \.\w* is not supported", TestName = "when unsupported extension message")]
        [TestCase("", "Path is null or empty", TestName = "when empty path")]
        [TestCase(null, "Path is null or empty", TestName = "when null")]
        public void ReturnsResultWithCorrectErrorMessage(string path, string expectedError)
        {
            A.CallTo(() => reader.Extension).Returns(".txt");
            sut = new FileReadersResolver(reader);
            sut.Get(path).Error.Should().MatchRegex(expectedError);
        }
    }
}