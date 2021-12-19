using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.FileReader;

namespace TagsCloudContainer_Tests
{
    [TestFixture]
    public class TxtFileReader_Should
    {
        private const string TestFilePath = @"..\..\..\TestFiles\testWords.txt";

        private readonly string[] expectedOutput =
        {
            "гитара",
            "скрипка",
            "  Инструмент",
            "Был",
            "Удар",
            "Я",
            "Играл",
            "Смотрел",
            "снова"
        };

        private readonly TxtFileReader sut = new();

        [Test]
        public void ReadFile_WhenExists()
        {
            var result = sut.ReadWords(TestFilePath);
            result.GetValueOrThrow().Should().BeEquivalentTo(expectedOutput);
        }

        [Test]
        public void ReturnsFailResult_WhenFileDoesNotExist()
        {
            var result = sut.ReadWords("notexist");
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReturnNothing_WhenEmptyFile()
        {
            sut.ReadWords(@"..\..\..\TestFiles\empty.txt").GetValueOrThrow()
                .Should()
                .BeEmpty();
        }
    }
}